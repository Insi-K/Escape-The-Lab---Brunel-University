using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Variables
    //Input Variables
    PlayerInput playerInput;
    Player player;

    public Vector2 RawMovementInput { get; private set; }

    //Normalize RawMovementInput variables
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool ShootInput { get; private set; }
    public bool MeleeInput { get; private set; }
    public bool ReleasedTriggerInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;

    private float shootInputStartTime;

    private float meleeInputStartTime;

    //Misc Variables
    public UI_Gameplay gameMenu;
    #endregion

    //Functions

    #region Unity Callback Functions
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckShootInputHoldTime();
        CheckMeleeInputHoldTime();
        //Debug.Log(playerInput.currentActionMap);
    }
    #endregion

    //--Input read Functions--

    #region Move Input
    //Move Action Functions
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }
    #endregion

    #region Jump Input
    //Jump Action Functions
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }
    }

        //to be used outside the class, sets the boolean to false (an instant later)
    public void UseJumpInput() => JumpInput = false;

        //keep the Jump input for a short period of time (this removes a bug where the player jumps with delay)
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
    #endregion

    #region Shoot Input
    //Shoot Input Functions
    public void OnShootInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            ReleasedTriggerInput = false;
            ShootInput = true;
        }

        if(context.canceled)
        {
            ReleasedTriggerInput = true;
            shootInputStartTime = Time.time;
        }
    }

        //Function used to reset ShootInput on Shoot state
    public void UseShootInput() => ShootInput = false;

        //Prevents from automatically change state on landing if given ShootInput during InAir state.
    private void CheckShootInputHoldTime()
    {
        if(ReleasedTriggerInput && ShootInput)
        {
            if (Time.time >= shootInputStartTime + inputHoldTime)
            {
                ShootInput = false;
                //Debug.Log("Shoot Disabled");
            }
        }
    }
    #endregion

    #region Menu Input
    //This method is used in both Gameplay and Menu ActionMaps
    public void OnMenuInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log(gameMenu.canUseMenu);
            PauseGame(); //Menu works flawlessly. Player can open and close it with keybind or with mouse
        }
    }

    private void PauseGame()
    {
        if(gameMenu.canUseMenu)
        {
            if (gameMenu.isMenuOpened)
            {
                gameMenu.Resume();
            }
            else
            {
                gameMenu.OpenMenu();
            }
        }
    }
    #endregion

    #region Melee Input
    public void OnMeleeInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            MeleeInput = true;
            meleeInputStartTime = Time.time;
        }
    }

    private void CheckMeleeInputHoldTime()
    {
        if(Time.time >= meleeInputStartTime + inputHoldTime)
        {
            MeleeInput = false;
        }
    }

    public void UseMeleeInput() => MeleeInput = false;
    #endregion

    #region Interact Input
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            player.UseKey();
        }
    }
    #endregion

    #region Throw Input
    public void OnThrowInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            player.ThrowGrenade();
        }
    }
    #endregion
}
