using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCombat : EnemyCombat
{
    [HideInInspector]
    public int prevState;
    [HideInInspector]
    public StateMovement myStateMovement;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        prevState = GetState();
    }

    // Get the current state by calling the state of the movement
    public int GetState()
    {
        return myStateMovement.GetState();
    }

    // Set the current state by setting the state of the movement
    public void SetState(int newState)
    {
        myStateMovement.SetState(newState);
    }

    // Return the target time into a variable (similar to StateMovement, but for time-based cooldowns in general)
    public float SetTarget(float elapsedTime)
    {
        return Time.time + elapsedTime;
    }

    // Return whether the timer has passed the goal time (ideal when used with a return value from SetTarget)
    public bool OnTarget(float goalTime)
    {
        return (Time.time > goalTime);
    }
}
