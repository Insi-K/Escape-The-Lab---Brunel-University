using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Substate: Move -- Player moves
public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    #region Player State Overrides
    public override void DoChecks()
    {
        base.DoChecks();
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

        player.CheckIfShouldFlip(xInput);

        player.SetVelocityX(playerData.movementVelocity * xInput);

        if(xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        if(yInput == -1)
        {
            xInput = 0;
            player.SetVelocityX(0f);
            stateMachine.ChangeState(player.CrouchState);
        }
        if(MeleeInput)
        {
            xInput = 0;
            player.SetVelocityX(0f);
            stateMachine.ChangeState(player.MeleeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    #endregion
}
