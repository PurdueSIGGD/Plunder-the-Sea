using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChange : ProjectileModifier
{
    // A projectile with SpeedChange can speed up or slow down over the course of its existence. Similar to SizeChange.

    public float startSpeed = 1.0f;        // The minimum speed multiplier (multiples of the projectile's base speed)
    public float endSpeed = 1.5f;        // The maximum speed multiplier (multiples of the projectile's base speed)
    public float timeToMax = 2.0f;      // The time to elapse between the projectile going from the minimum to the maximum

    private float startTime = 0.0f;        // The starting time, in seconds
    private float baseSpeed = 0.0f;         // The base speed to base speed changes on
    private Rigidbody2D rb;                 // The rigid body to change the speed of
    private Projectile p;                   // The parent projectile

    public override void ProjectileStart()
    {
        // Initialize variables
        rb = GetComponent<Rigidbody2D>();
        p = GetComponent<Projectile>();
        baseSpeed = p.speed;

        if (enabled)
        {
            AlterSpeed(startSpeed);
        }
    }

    public override void ProjectileUpdate()
    {
        if (enabled)
        {
            if (startTime == 0.0f)
            {
                startTime = Time.time;
            }

            float diffTime = Time.time - startTime;
            AlterSpeed(Mathf.Lerp(startSpeed, endSpeed, diffTime / timeToMax));
        }
    }

    void AlterSpeed(float newSpeed)
    {
        Vector3 angle = rb.velocity.normalized;
        rb.velocity = angle * newSpeed * baseSpeed;
        p.speed = newSpeed;
    }
}
