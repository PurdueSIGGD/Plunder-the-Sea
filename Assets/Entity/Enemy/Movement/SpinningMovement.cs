using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningMovement : StateMovement
{
    // the amount of time that the enemy spins
    public float spinTime = 2f;

    // the speed that the enemy rotates
    public float spinSpeed = 30f;

    // amount of time enemy stays dizzy after spinning toward player
    public float dizzyTime = 2f;

    // how long an action has been going
    private float time = 0f;

    // The state for approaching. There are 3 options, which represent different points in movement.
    public enum SpinningState
    {
        spinning,
        dizzy
    }
    public SpinningState spinningState = SpinningState.spinning;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            switch (spinningState)
            {
                case SpinningState.spinning:
                    if (spinTime > time)
                    {
                        time += Time.deltaTime;
                        // moves toward player while spinning
                        MoveTowards();
                    }
                    else
                    {
                        spinningState = SpinningState.dizzy;
                        time = 0f;
                    }
                    break;

                case SpinningState.dizzy:
                    if (dizzyTime > time)
                    {
                        time += Time.deltaTime;
                        // stops movement
                        myBase.myRigid.velocity = Vector2.zero;
                    }
                    else
                    {
                        spinningState = SpinningState.spinning;
                        time = 0f;
                    }
                    break;
            }
        }
    }
}