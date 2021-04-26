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

    private static LayerMask mask;

    // Very simple state
    public enum SweepState
    {
        isSweeping = 0,
        normal = 1
    }
    public SweepState sweepState = SweepState.normal;

    // The current angle
    private Vector3 moveAngle = Vector3.zero;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            switch (sweepState)
            {
                case SweepState.normal:
                    if (PlayerDistance() <= sweepDistance && !CheckForWalls())
                    {
                        // Change state and initialize going the correct angle if close enough
                        sweepState = SweepState.isSweeping;
                        moveAngle = PlayerAngle();
                    }
                    MoveTowards();
                    break;

                case SweepState.isSweeping:
                    if (PlayerDistance() > sweepDistance || CheckForWalls())
                    {
                        sweepState = SweepState.normal;
                    }
                    // Move in the direction of moveAngle, then try to make the angle go more towards the player
                    myBase.myRigid.velocity = moveAngle * myBase.myStats.movementSpeed;

                    if (PlayerAngle() != Vector3.zero)
                    {
                        moveAngle = Vector3.RotateTowards(moveAngle, PlayerAngle(), angleChange * Time.deltaTime, 0.0f);
                        moveAngle.Normalize();
                    }
                    break;
            }
        }
    }

    bool CheckForWalls()
    {
        float scale = transform.localScale.x * 0.15f;
        return Physics2D.Linecast(transform.position + new Vector3(scale, 0 ,0), myBase.player.transform.position, mask) ||
            Physics2D.Linecast(transform.position + new Vector3(-scale, 0, 0), myBase.player.transform.position, mask) ||
            Physics2D.Linecast(transform.position + new Vector3(0, scale, 0), myBase.player.transform.position, mask) ||
            Physics2D.Linecast(transform.position + new Vector3(0, -scale, 0), myBase.player.transform.position, mask);
    }

    public override int GetState()
    {
        return (int)this.sweepState;
    }

    public override void SetState(int newState)
    {
        this.sweepState = (SweepState)newState;
    }
}
