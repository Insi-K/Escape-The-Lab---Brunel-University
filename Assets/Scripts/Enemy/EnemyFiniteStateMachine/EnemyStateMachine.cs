using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    //Start State Machine to the default state (defined by the enemy obj)
    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.SetTimer(1f);
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState, float time)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.SetTimer(time);
        CurrentState.Enter();
    }
}
