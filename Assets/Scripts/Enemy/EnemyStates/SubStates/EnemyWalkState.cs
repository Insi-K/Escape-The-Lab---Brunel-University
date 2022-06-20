using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyPeacefulState
{
    public EnemyWalkState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
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

        enemy.SetVelocityX(2f * xDirection);

        if (ledgeDetected == xDirection) //If ledge detected and xDirection is the same side
        {
            enemy.SetVelocityX(0);
            stateMachine.ChangeState(enemy.IdleState, 3f);
        }
        else if (isOnTrace)
        {
            stateMachine.ChangeState(enemy.AimShootState, 0.5f);
        }
        else if(!isOnTrace && isHostile)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
