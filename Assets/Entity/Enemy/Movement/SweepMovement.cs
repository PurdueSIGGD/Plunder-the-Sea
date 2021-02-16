using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepMovement : StateMovement
{
    // The angle change speed, in radians per second
    public float angleChange = 1.5f;

    // The distance from the player the enemy starts sweep-moving.
    // Ideally large enough to make wide turns, but small enough to not run into walls.
    public float sweepDistance = 8.0f;

    // Very simple state
    public enum SweepState
    {
        isSweeping = 0,
        normal = 1
    }
    public SweepState sweepState = SweepState.normal;

    // The current angle
    private Vector3 moveAngle = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            switch (sweepState)
            {
                case SweepState.normal:
                    if (PlayerDistance() <= sweepDistance)
                    {
                        // Change state and initialize going the correct angle if close enough
                        sweepState = SweepState.isSweeping;
                        moveAngle = PlayerAngle();
                    }
                    MoveTowards();
                    break;

                case SweepState.isSweeping:
                    if (PlayerDistance() > sweepDistance)
                    {
                        sweepState = SweepState.normal;
                    }
                    // Move in the direction of moveAngle, then try to make the angle go more towards the player
                    myBase.myRigid.velocity = moveAngle * myBase.myStats.movementSpeed;

                    moveAngle = Vector3.RotateTowards(moveAngle, PlayerAngle(), angleChange * Time.deltaTime, 0.0f);
                    moveAngle.Normalize();

                    break;
            }
        }
    }
}
