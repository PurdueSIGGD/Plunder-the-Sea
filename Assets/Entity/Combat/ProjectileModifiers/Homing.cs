using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : ProjectileModifier
{
    // A modifier to allow projectiles to curve towards the player.

    public GameObject target = null;            // The game object to home in on. For enemies, is almost always the player
    public bool targetPlayer = true;            // A shortcut to make this automatically target the player
    public float angleChange = 1.5f;            // The angle change speed, in radians per second

    private Rigidbody2D rb;                     // The rigid body to change the speed of
    public Vector3 moveAngle = Vector3.zero;   // The angle that the projectile is moving in
    private Projectile p;                       // The parent projectile

    public override void ProjectileStart()
    {
        // Initialize variables
        rb = GetComponent<Rigidbody2D>();
        p = GetComponent<Projectile>();
        moveAngle = rb.velocity.normalized;

        if (targetPlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private Vector3 TargetAngle()
    {
        if (target != null)
        {
            return (target.transform.position - transform.position).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public override void ProjectileUpdate()
    {
        if (enabled)
        {
            // Move in the direction of moveAngle, then try to make the angle go more towards the player
            rb.velocity = moveAngle * p.speed;

            if (TargetAngle() != Vector3.zero)
            {
                moveAngle = Vector3.RotateTowards(moveAngle, TargetAngle(), angleChange * Time.deltaTime, 0.0f);
                moveAngle.Normalize();
            }
        }
        
    }
}
