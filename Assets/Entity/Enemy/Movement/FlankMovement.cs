using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlankMovement : StateMovement
{
    // The minimum distance from the player to try to maintain.
    public float minDistance = 4.0f;

    // The maximum distance from the player to try to maintain.
    public float maxDistance = 5.0f;

    // The minimum time spent moving (in seconds). After this, it will advance state immediately when in range.
    public float movingTime = 2.0f;

    // The amount of time the enemy stays still (in seconds) between moving cycles. This is probably when it would attack.
    public float stationaryTime = 2.0f;

    // The state of the flanking enemy. There are 2 options, which resemble different points in its movement.
    public enum FlankState
    {
        flanking,
        stationary
    }
    public FlankState flankState = FlankState.flanking;



    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            switch (flankState)
            {
                case FlankState.flanking:
                    // Check if in flanking range
                    if (minDistance <= PlayerDistance() && maxDistance >= PlayerDistance())
                    {
                        // If the minimum time has passed
                        if (OnTarget())
                        {
                            // Set target and change state
                            flankState = FlankState.stationary;
                            SetTarget(stationaryTime);
                        }
                        else
                        {
                            // Stay still if within the range but the minimum flanking time hasn't passed yet
                            myBase.myRigid.velocity = Vector3.zero;
                        }
                    }
                    else if (minDistance > PlayerDistance())
                    {
                        // Try to get more distance
                        MoveAway();
                    }
                    else
                    {
                        // Try to get closer
                        MoveTowards();
                    }
                    break;

                case FlankState.stationary:
                    // Check if it should be on the next state (without moving)
                    myBase.myRigid.velocity = Vector3.zero;

                    if (OnTarget())
                    {
                        flankState = FlankState.flanking;
                        SetTarget(movingTime);
                    }
                    break;
            }
        }
    }
}