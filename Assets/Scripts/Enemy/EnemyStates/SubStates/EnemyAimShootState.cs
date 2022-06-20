using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimShootState : EnemyHostileState
{
    public EnemyAimShootState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
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
        if(isOnTrace && stateTimer > 0)
        {
            if(Time.time > startTime + stateTimer)
            {
                stateMachine.ChangeState(enemy.ShootState);
            }
        }
        if(!isOnTrace)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
