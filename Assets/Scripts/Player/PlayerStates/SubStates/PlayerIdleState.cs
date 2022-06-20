using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Substate: Idle -- Player does not move
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        player.SetVelocityX(0f);

        if (xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if(yInput == -1)
        {
            xInput = 0;
            stateMachine.ChangeState(player.CrouchState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    #endregion
}
