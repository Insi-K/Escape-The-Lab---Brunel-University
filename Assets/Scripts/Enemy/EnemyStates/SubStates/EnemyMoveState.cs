using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyHostileState
{
    public EnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
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

        enemy.SetVelocityX(enemyData.runSpeed * xDirection);

        if (ledgeDetected == xDirection) //If ledge detected and xDirection is the same side
        {
            enemy.SetHostile(false);
            enemy.SetVelocityX(0);
            stateMachine.ChangeState(enemy.IdleState, 3f);
        }

        if(isOnTrace)
        {
            enemy.SetVelocityX(0);
            stateMachine.ChangeState(enemy.AimShootState, 0.5f);
        }

        if (Mathf.Abs(distance) <= 1f)
        {
            //enemy.SetHostile(false);
            enemy.SetVelocityX(0);
            stateMachine.ChangeState(enemy.IdleState, 0.5f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
