using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyPeacefulState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

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

        enemy.CheckIfShouldFlip(xDirection);

        if (isOnTrace)
        {
            stateMachine.ChangeState(enemy.AimShootState, 0.5f);
        }
        else if (!isHostile && stateTimer != 0)
        {
            if (Time.time >= startTime + stateTimer)
            {
                stateMachine.ChangeState(enemy.WalkState);
            }
        }
        else if (isHostile && !isOnTrace && stateTimer == 0)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
        else if(isHostile && stateTimer != 0 && Mathf.Abs(distance) <= 1f)
        {
            if (Time.time >= startTime + stateTimer)
            {
                stateMachine.ChangeState(enemy.MeleeState, 0.5f);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
