using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAbilityState
{
    private int xInput;

    public PlayerMeleeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0);
        player.InputHandler.UseMeleeInput();
        player.DoAttack();
        isAnimationFinished = true; //This should be replaced with AnimationFinishTrigger();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        player.CheckIfShouldFlip(xInput);

        if(isAnimationFinished && Time.time >= startTime + 0.35f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
