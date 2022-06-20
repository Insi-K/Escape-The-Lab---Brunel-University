using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SuperState: Grounded State -- All substates that share a ground check
public class PlayerGroundedState : PlayerState
{
    //Local variables
    protected int xInput;
    protected int yInput; //For CrouchInput

    protected bool isTouchingCeiling; //used to (!)exit crouch state

    private bool JumpInput;
    private bool isGrounded;

    protected bool ShootInput;

    protected bool MeleeInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    #region Player State Overrides
    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingCeiling = player.CheckForCeiling();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        JumpInput = player.InputHandler.JumpInput;
        ShootInput = player.InputHandler.ShootInput;
        MeleeInput = player.InputHandler.MeleeInput;

        if(JumpInput && stateMachine.CurrentState != player.AimShootState && stateMachine.CurrentState != player.CrouchState)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if(ShootInput && stateMachine.CurrentState != player.AimShootState)
        {
            //Player must stop before changing state
            xInput = 0;
            stateMachine.ChangeState(player.AimShootState);
        }
        else if(MeleeInput && stateMachine.CurrentState != player.AimShootState && stateMachine.CurrentState != player.CrouchState)
        {
            xInput = 0;
            stateMachine.ChangeState(player.MeleeState);
        }
        else if(!isGrounded && Mathf.Abs(player.CurrentVelocity.y) > 0.01f)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    #endregion
}
