using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMovement : StateMovement
{
    // The state of the no movement enemy. There are 2 options, which resemble different points in its movement.
    public enum MovementState
    {
        none
    }
    public MovementState movementState = MovementState.none;



    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            switch (movementState)
            {
                case MovementState.none:
                    myBase.myRigid.velocity = Vector3.zero;
                    break;
            }
        }
    }
}
