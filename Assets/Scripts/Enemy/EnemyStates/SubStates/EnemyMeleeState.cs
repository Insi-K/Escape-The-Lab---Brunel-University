using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeState : EnemyAbilityState
{
    public EnemyMeleeState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.DoAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(stateTimer > 0)
        {
            if(Time.time >= startTime + stateTimer)
            {
                if(!GameObject.FindWithTag("Player"))
                {
                    enemy.SetHostile(false);
                }
                stateMachine.ChangeState(enemy.IdleState, 0.5f);
            }
        }
    }

}
