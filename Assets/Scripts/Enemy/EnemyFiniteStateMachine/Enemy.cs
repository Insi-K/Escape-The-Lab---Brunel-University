using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Enemy Base Variables
    public int Hp { get; private set; }
    public GameObject hpBar;

    private float hpBarOrigSize;

    [SerializeField] private GameObject attackBox;
    #endregion

    #region State Variables
    public EnemyStateMachine StateMachine { get; private set; }

    //Enemy States are declared HERE:
    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemyWalkState WalkState { get; private set; }
    public EnemyAimShootState AimShootState { get; private set; }
    public EnemyShootState ShootState { get; private set; }
    public EnemyMeleeState MeleeState { get; private set; }

    [SerializeField] private EnemyData enemyData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    //NOTE: Enemy does not use an InputHandler.
    public Rigidbody2D RB { get; private set; }
    //NOTE: Enemy does not use a MovementCollider.
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    //workspace is a temporary variable used in all Functions that manipulate Vector2
    private Vector2 workspace;

    public GameObject bullet;
    public Transform gunPoint;

    public GameObject key;
    public bool hasKey;

    public GameObject DeathParticle;
    #endregion

    #region Enemy Behaviour Variables
    //Reference to player Obj;
    private GameObject player;

    //Navigation variables
    private RaycastHit2D rightLedge;
    private RaycastHit2D leftLedge;
    private RaycastHit2D rightWall;
    private RaycastHit2D leftWall;

    private RaycastHit2D rightTrace;
    private RaycastHit2D leftTrace;

    private float rightTraceDist;
    private float leftTraceDist;

    public float distance;

    public bool IsEnemyHostile { get; private set; }
    #endregion

    #region Debug Variables
    [Header("--Show Debug--")]
    [SerializeField] public bool DebugStates;
    [SerializeField] public bool DebugLedgeCheck;
    [SerializeField] public bool DebugHostileRange;

    private Vector2 enemyOrigin;
    private Vector2 rayCastOrigin;
    private Vector2 rayCastTarget;



    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "idle");
        MoveState = new EnemyMoveState(this, StateMachine, enemyData, "move");
        WalkState = new EnemyWalkState(this, StateMachine, enemyData, "walk");
        AimShootState = new EnemyAimShootState(this, StateMachine, enemyData, "aimShoot");
        ShootState = new EnemyShootState(this, StateMachine, enemyData, "shoot");
        MeleeState = new EnemyMeleeState(this, StateMachine, enemyData, "melee");

        rightTraceDist = enemyData.frontTraceDistance;
        leftTraceDist = enemyData.backTraceDistance;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        FacingDirection = 1;

        Hp = enemyData.maxHp;
        hpBarOrigSize = hpBar.transform.localScale.x;

        //Initialize State Machine into default state (Idle)
        StateMachine.Initialize(IdleState);

        //Debug.Log("Starting HP: " + Hp);
    }

    private void Update()
    {
        CheckHealth();
        UpdateUI();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDrawGizmos()
    {
        enemyOrigin = transform.position;

        //ShootTrace
        rayCastOrigin.Set(enemyOrigin.x + enemyData.rayCastOffsetX, enemyOrigin.y + enemyData.rayCastOffsetY);
        rayCastTarget.Set(rayCastOrigin.x + rightTraceDist, rayCastOrigin.y);
        Gizmos.DrawLine(rayCastOrigin, rayCastTarget);

        rayCastOrigin.Set(enemyOrigin.x - enemyData.rayCastOffsetX, enemyOrigin.y + enemyData.rayCastOffsetY);
        rayCastTarget.Set(rayCastOrigin.x - leftTraceDist, rayCastOrigin.y);
        Gizmos.DrawLine(rayCastOrigin, rayCastTarget);

        //LedgeCheck
        rayCastOrigin.Set(enemyOrigin.x + enemyData.ledgeOffsetX, enemyOrigin.y - enemyData.ledgeOffsetY);
        rayCastTarget.Set(rayCastOrigin.x, rayCastOrigin.y - enemyData.ledgeCheckLength);
        Gizmos.DrawLine(rayCastOrigin, rayCastTarget);

        rayCastOrigin.Set(enemyOrigin.x - enemyData.ledgeOffsetX, enemyOrigin.y - enemyData.ledgeOffsetY);
        rayCastTarget.Set(rayCastOrigin.x, rayCastOrigin.y - enemyData.ledgeCheckLength);
        Gizmos.DrawLine(rayCastOrigin, rayCastTarget);

        //WallCheck
        rayCastOrigin.Set(enemyOrigin.x, enemyOrigin.y - enemyData.ledgeOffsetY);

        rayCastTarget.Set(rayCastOrigin.x + enemyData.ledgeCheckLength, rayCastOrigin.y);
        Gizmos.DrawLine(rayCastOrigin, rayCastTarget);

        rayCastTarget.Set(rayCastOrigin.x - enemyData.ledgeCheckLength, rayCastOrigin.y);
        Gizmos.DrawLine(rayCastOrigin, rayCastTarget);


    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetHostile(bool cState) => IsEnemyHostile = cState;

    //NOTE: Enemy currently does not need SetVelocityY() as it uses horizontal movement.
    #endregion

    #region Check Functions
    public int CheckDirection()
    {
        if(!rightLedge)
        {
            return -1;
        }
        if(!leftLedge)
        {
            return 1;
        }
        return FacingDirection;
    }
    public void CheckIfShouldFlip(int targetDirection)
    {
        if(targetDirection != 0 && targetDirection != FacingDirection)
        {
            Flip();
            ChangeTraceDirection(FacingDirection);
        }
    }
    public void ChangeTraceDirection(int Direction)
    {
        if(Direction == 1)
        {
            rightTraceDist = enemyData.frontTraceDistance;
            leftTraceDist = enemyData.backTraceDistance;
        }
        else if(Direction == -1)
        {
            rightTraceDist = enemyData.backTraceDistance;
            leftTraceDist = enemyData.frontTraceDistance;
        }
    }
    public int CheckForLedge()
    {
        //Create 2 new Raycasts
        workspace.Set(transform.position.x + enemyData.ledgeOffsetX, transform.position.y - enemyData.ledgeOffsetY);
        rightLedge = Physics2D.Raycast(workspace, Vector2.down, enemyData.ledgeCheckLength, enemyData.whatIsGround);

        workspace.Set(transform.position.x - enemyData.ledgeOffsetX, transform.position.y - enemyData.ledgeOffsetY);
        leftLedge = Physics2D.Raycast(workspace, Vector2.down, enemyData.ledgeCheckLength, enemyData.whatIsGround);

        workspace.Set(transform.position.x, transform.position.y - enemyData.ledgeOffsetY);
        rightWall = Physics2D.Raycast(workspace, Vector2.right, enemyData.ledgeCheckLength, enemyData.whatIsGround);
        leftWall = Physics2D.Raycast(workspace, Vector2.left, enemyData.ledgeCheckLength, enemyData.whatIsGround);

        int ledgeDetected = 0;
        if (!rightLedge || rightWall) ledgeDetected = 1;
        else if (!leftLedge || leftWall) ledgeDetected = -1;
        else ledgeDetected = 0;

        //For Debug use only
        if (DebugLedgeCheck)
        {
            if (ledgeDetected != 0)
            {
                Debug.Log("Ledge Detected");
            }
            else
            {
                Debug.Log("Safe");
            }
        }

        return ledgeDetected;
    }
    public bool CheckShootTrace()
    {
        //Set upo the Shooting Trace Raycasts (left and right)
        workspace.Set(transform.position.x + enemyData.rayCastOffsetX, transform.position.y + enemyData.rayCastOffsetY);
        rightTrace = Physics2D.Raycast(workspace, Vector2.right, rightTraceDist, enemyData.whatIsPlayer);

        workspace.Set(transform.position.x - enemyData.rayCastOffsetX, transform.position.y + enemyData.rayCastOffsetY);
        leftTrace = Physics2D.Raycast(workspace, Vector2.left, leftTraceDist, enemyData.whatIsPlayer);

        if(rightTrace || leftTrace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float CheckDistanceToPlayer()
    {
        distance = player.transform.position.x - transform.position.x;
        return distance;
    }

    private void CheckHealth()
    {
        if(Hp < 100)
        {
            hpBar.SetActive(true);
        }
        else
        {
            hpBar.SetActive(false);
        }
        if(Hp <= 0)
        {
            Instantiate(DeathParticle, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            if(hasKey) Instantiate(key, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            hpBar.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Other Functions
    private void EnemyAnimationTrigger() => StateMachine.CurrentState.EnemyAnimationTrigger();
    private void EnemyAnimationFinishTrigger() => StateMachine.CurrentState.EnemyAnimationFinishTrigger();
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    public void SpawnBullet()
    {
        if(FacingDirection == 1)
        {
            GameObject bInst = Instantiate(bullet, gunPoint.position, Quaternion.Euler(new Vector3(0,0,0)));
        }
        else if(FacingDirection == -1)
        {
            GameObject bInst = Instantiate(bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
       
        BulletController bulletController = bullet.GetComponent<BulletController>();
    }

    public void Hit(int damage)
    {
        Hp -= damage;
        IsEnemyHostile = true;
        //Debug.Log("HP left " + Hp);
    }

    public void DoAttack() => StartCoroutine(Attack());

    public IEnumerator Attack()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(enemyData.attackDuration);
        attackBox.SetActive(false);
    }

    private void UpdateUI()
    {
        hpBar.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.8f);

        if (hpBar.activeInHierarchy)
        {
            hpBar.transform.localScale = new Vector2(hpBarOrigSize * (float)Hp / (float)enemyData.maxHp, hpBar.transform.localScale.y);
        }
    }
    #endregion



}
