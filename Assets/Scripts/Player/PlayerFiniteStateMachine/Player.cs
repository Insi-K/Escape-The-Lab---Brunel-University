using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Player Variables
    public int Hp { get; private set; }
    public int Ammo { get; private set; }

    public bool canUseKey;

    public Image healthBar;
    public Image ammoBar;
    public Image KeySprite;
    public Text GrenadeTxt;

    private Vector2 healthBarOrigSize;
    private Vector2 ammoBarOrigSize;

    [SerializeField] private GameObject attackBox;

    public Transform grenadePoint;
    private int grenadeCount;
    public GameObject grenadePrefab;
    #endregion

    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerAimShootState AimShootState { get; private set; }
    public PlayerShootState ShootState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    public PlayerMeleeState MeleeState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    #endregion

    #region Check Transforms
    [Header("--Transform Variables--")]
    [SerializeField] private Transform ceilingCheck;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private Transform gunPoint;
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workspace;

    public GameObject DeathParticle;

    public GameObject bullet;

    public UI_Gameplay gameUI;

    public DoorController currentDoor;

    [Header("--Show Debug--")]
    public bool DebugStates;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "inAir");
        AimShootState = new PlayerAimShootState(this, StateMachine, playerData, "aimShoot");
        ShootState = new PlayerShootState(this, StateMachine, playerData, "shoot");
        CrouchState = new PlayerCrouchState(this, StateMachine, playerData, "crouch");
        MeleeState = new PlayerMeleeState(this, StateMachine, playerData, "melee");
    }

    private void Start()
    {
        //Script Setup
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<BoxCollider2D>();

        //Player Setup
        FacingDirection = 1;

        Hp = playerData.maxHp;
        Ammo = 0;
        grenadeCount = 0;

        healthBarOrigSize = healthBar.rectTransform.sizeDelta;
        ammoBarOrigSize = ammoBar.rectTransform.sizeDelta;

        UpdateUI();
        
        //Initialize State Machine into default state (Idle)
        StateMachine.Initialize(IdleState);

        //This is used to disable and enable the player input. May come in handy for starting scenes.
        /*PlayerInput input = GetComponent<PlayerInput>();
        input.actions.Disable();*/
        //Debug.Log("Starting HP: " + Hp);
        //Debug.Log("Starting Ammo: " + Ammo);
    }

    private void Update()
    {
        CheckHealth();
        UpdateUI();
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DoorController door))
        {
            currentDoor = door;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DoorController door))
        {
            currentDoor = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        Gizmos.DrawWireSphere(ceilingCheck.position, 0f);
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    #endregion

    #region Check Functions

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, 0f, playerData.whatIsGround);
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Player Update Functions
    private void CheckHealth()
    {
        if(Hp <= 0)
        {
            Instantiate(DeathParticle, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
            gameObject.SetActive(false);
            gameUI.Restart();
        }
    }

    private void UpdateUI()
    {
        //Set healthBar
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarOrigSize.x * (float) Hp/ (float) playerData.maxHp, healthBar.rectTransform.sizeDelta.y);

        //Set ammoBar
        ammoBar.rectTransform.sizeDelta = new Vector2(ammoBarOrigSize.x * (float) Ammo / (float) playerData.maxAmmo, ammoBar.rectTransform.sizeDelta.y);

        //Set Grenade Amount
        GrenadeTxt.text = grenadeCount.ToString();
    }
    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void SpawnBullet()
    {
        Ammo -= 1;
        //Debug.Log("Ammo left: " + Ammo);
        if (FacingDirection == 1)
        {
            Instantiate(bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else if (FacingDirection == -1)
        {
            Instantiate(bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
    }

    public void DoAttack() => StartCoroutine(Attack());

    public IEnumerator Attack()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(playerData.attackDuration);
        attackBox.SetActive(false);
    }

    public void Hit(int damage)
    {
        Hp -= damage;
        UpdateUI();
    }

    public void CurePlayer(int heal)
    {
        Hp += heal;
        if(Hp > playerData.maxHp)
        {
            Hp = playerData.maxHp;
        }
        UpdateUI();
    }

    public void ReloadAmmo(int amount)
    {
        Ammo += amount;
        if(Ammo > playerData.maxAmmo)
        {
            Ammo = playerData.maxAmmo;
        }
        UpdateUI();
    }

    public void ObtainKey()
    {
        canUseKey = true;
        KeySprite.gameObject.SetActive(true);
    }

    public void UseKey()
    {
        if(currentDoor != null && canUseKey)
        {
            canUseKey = false;
            KeySprite.gameObject.SetActive(false);
            currentDoor.OpenDoor();
            currentDoor = null;
        }
    }

    public void addGrenade()
    {
        ++grenadeCount;
        UpdateUI();
    }

    public void ThrowGrenade()
    {
        if(grenadeCount > 0)
        {
            GameObject grenade = Instantiate(grenadePrefab, grenadePoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            grenade.GetComponent<Rigidbody2D>().velocity = grenadePoint.transform.right * 15f;
            --grenadeCount;
            UpdateUI();
        }
    }
    #endregion
}
