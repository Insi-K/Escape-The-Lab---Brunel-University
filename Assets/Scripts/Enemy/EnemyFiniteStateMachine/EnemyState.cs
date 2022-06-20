using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    //Class Variables
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected EnemyData enemyData;

    protected bool isAnimationFinished;

    protected float startTime;

    private string animBoolName;

    //Other variables
    protected int xDirection; //Equivalent to player's xInput
    protected int ledgeDetected;
    protected float stateTimer;
    protected bool useTimer;
    protected bool isHostile;
    protected bool isOnTrace;
    protected float distance;

    //Class Constructor

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.enemyData = enemyData;
        this.animBoolName = animBoolName;
    }

    //Base Functions

    //Function called when the enemy enters into a new state
    public virtual void Enter()
    {
        DoChecks();
        enemy.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        if(enemy.DebugStates) Debug.Log(animBoolName);
        isAnimationFinished = false;
        xDirection = enemy.CheckDirection();
    }

    //Function called when the enemy exits the current state
    public virtual void Exit()
    {
        enemy.Anim.SetBool(animBoolName, false);
        xDirection = enemy.CheckDirection();
        stateTimer = 0;
    }

    //Equivalent to Update()
    public virtual void LogicUpdate()
    {
        ledgeDetected = enemy.CheckForLedge();
        isOnTrace = enemy.CheckShootTrace();
        isHostile = enemy.IsEnemyHostile;
        distance = enemy.CheckDistanceToPlayer();
        if(isHostile)
        {
            if (distance < 0)
            {
                xDirection = -1;
            }
            else if (distance > 0)
            {
                xDirection = 1;
            }
        }
        
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

    public virtual void EnemyAnimationTrigger() { }

    public virtual void EnemyAnimationFinishTrigger() => isAnimationFinished = true;

    public void SetTimer(float time)
    {
        stateTimer = time;
    }
}
