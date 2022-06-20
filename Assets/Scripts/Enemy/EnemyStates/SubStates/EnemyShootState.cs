using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : EnemyAbilityState
{
    public EnemyShootState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SpawnBullet();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(enemy.AimShootState, 0.5f);
        }
    }
}
