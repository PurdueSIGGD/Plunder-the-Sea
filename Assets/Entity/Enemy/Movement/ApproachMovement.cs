using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Approaches the player, then charges up an action (which has a cooldown) if it is still in range.
public class ApproachMovement : StateMovement
{
    // The distance to approach to before acting
    public float approachDistance = 0.5f;

    // The charge up time to act
    public float actChargeUpTime = 1.0f;

    // The cooldown after acting (to account for the time the act takes)
    public float actCooldownTime = 1.0f;

    // The state for approaching. There are 3 options, which represent different points in movement.
    public enum ApproachState
    {
        approaching = 0,
        activating = 1,
        cooldown = 2
    }
    public ApproachState approachState = ApproachState.approaching;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            switch (approachState)
            {
                case ApproachState.approaching:
                    // Check if reached range
                    if (PlayerDistance() <= approachDistance)
                    {
                        approachState = ApproachState.activating;
                        SetTarget(actChargeUpTime);
                    }
                    else
                    {
                        MoveTowards();
                    }
                    break;
                case ApproachState.activating:
                    if (OnTarget())
                    {
                        approachState = ApproachState.cooldown;
                        SetTarget(actCooldownTime);
                    }
                    break;
                case ApproachState.cooldown:
                    if (OnTarget())
                    {
                        approachState = ApproachState.approaching;
                    }
                    break;
            }
        }
    }

    public override int GetState()
    {
        return (int)this.approachState;
    }
}