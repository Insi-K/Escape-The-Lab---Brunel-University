using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //Class variables
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;

    protected float startTime;

    private string animBoolName;

    //Class Constructor
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    #region Base Functions
    //Function called when the player enters into a new state
    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        if(player.DebugStates) Debug.Log(animBoolName);
        isAnimationFinished = false;
    }

    //Function called when the player exits the current state
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
    }

    //Equivalent to Update()
    public virtual void LogicUpdate()
    {

    }

    //Equivalent to FixedUpdate()
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    //Check for interactions
    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    #endregion


}
