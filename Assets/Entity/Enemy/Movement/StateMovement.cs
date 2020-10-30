using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Utility subclass for state-driven enemy movement
public class StateMovement : EnemyMovement
{
    // One-liner for distance from the player
    protected float PlayerDistance()
    {
        return Vector2.Distance(myBase.player.transform.position, myBase.transform.position);
    }

    // Move towards the player
    protected void MoveTowards()
    {
        if (moving)
        {
            myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
                * myBase.myStats.movementSpeed;
        }
    }

    // Move away from the player
    protected void MoveAway()
    {
        if (moving)
        {
            myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
                * -myBase.myStats.movementSpeed;
        }
    }

    // The target time for changing actions, used for time-driven states (charging up)
    private float targetTime = 0.0f;

    // Set a new target time
    protected void SetTarget(float delay)
    {
        targetTime = Time.time + delay;
    }

    // Check if the target time has passed
    protected bool OnTarget()
    {
        return (Time.time >= targetTime);
    }

    // Get the current state (overridden in subclasses)
    public virtual int GetState()
    {
        return 0;
    }
}