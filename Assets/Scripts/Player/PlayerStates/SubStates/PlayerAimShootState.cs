using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimShootState : PlayerGroundedState
{
    private bool ReleasedTrigger;
    private float stateHoldTime = 0.2f;
    public PlayerAimShootState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ReleasedTrigger = player.InputHandler.ReleasedTriggerInput;

        if(ShootInput)
        {
            if(ReleasedTrigger && player.Ammo > 0) //Player must have ammo to shoot
            {
                stateMachine.ChangeState(player.ShootState);
            }
            else if(xInput != 0)
            {
                player.CheckIfShouldFlip(xInput);
            }
        }
        else
        {
            if (Time.time - startTime >= stateHoldTime)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if(yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchState);
            }
        }
    }
}
