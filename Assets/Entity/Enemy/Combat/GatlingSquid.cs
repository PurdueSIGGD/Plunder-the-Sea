using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingSquid : StateCombat
{
    // Const values to make coding easier
    const int cooldown = (int)ApproachMovement.ApproachState.cooldown;
    const int activating = (int)ApproachMovement.ApproachState.activating;
    const int approaching = (int)ApproachMovement.ApproachState.approaching;

    // How long the gatling squid has to wait each time before firing
    public float firingCooldown = 0.1f;
    private float firingTracker = 0;

    // Max range that the Gatling Squid can fire from
    public float maxRange = 8f;

    // How many shots the gatling squid can fire before reloading, and how long it takes to reload
    public float clipSize = 20f;
    public float reloadTime = 1.5f;
    private float reloadTracker = 0;
    private float clipCounter = 0f;
    private bool reloading = false;

    // How much shots can spread, in degrees
    public float shotSpread = 10.0f;

    // How long before each time the gatling squid searches
    public float searchCooldown = 0.25f;
    private float searchTarget = 0;

    // Squid Bullet projectile
    public GameObject squidBullet;

    // Layer mask for scanning wall
    private static LayerMask mask;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        prevState = GetState();
        mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        // If the gatling squid should be reloading
        if (reloading)
        {
            if (OnTarget(reloadTracker))
            {
                // Exit once reloading is complete
                reloading = false;
                clipCounter = 0;
                myStateMovement.moving = true;
                return;
            }
            // Animate reloading as idle animation
            anim.SetInteger("State", 2);

            // Otherwise do nothing AI-wise, so reloading isn't interrupted
            return;
        }

        switch (current)
        {
            case cooldown:
                if (OnTarget(searchTarget))
                {
                   // Get closer to the enemy if they're hiding behind a wall
                   if (myBase.player == null || myStateMovement.PlayerDistance() > maxRange || Physics2D.Linecast(transform.position, myBase.player.transform.position, mask))
                    {
                        SetState(approaching);
                        searchTarget = SetTarget(searchCooldown);
                        break;
                    }
                    searchTarget = SetTarget(searchCooldown);
                }
                // Animate Shooting Sprites
                anim.SetInteger("State", 1);
                float angle = Vector2.Angle(Vector2.up, myStateMovement.PlayerAngle());
                if (angle <= 157.5)
                {
                    sprite.flipX = isPlayerLeft();
                } else
                {
                    sprite.flipX = false;
                }
                anim.SetBool("Mirror", sprite.flipX);

                if (angle <= 22.5)
                {
                    anim.SetInteger("Angle", 0);
                }
                else if (angle <= 67.5)
                {
                    anim.SetInteger("Angle", 1);
                }
                else if (angle <= 112.5)
                {
                    anim.SetInteger("Angle", 2);
                }
                else if (angle <= 157.5)
                {
                    anim.SetInteger("Angle", 3);
                }
                else
                {
                    anim.SetInteger("Angle", 4);
                }

                // Try to shoot if possible and the cooldown allows
                if (OnTarget(firingTracker) && clipCounter < clipSize)
                {
                    Projectile bullet = Shoot(squidBullet);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = Quaternion.AngleAxis(Random.Range(-shotSpread,shotSpread), Vector3.forward) * rb.velocity;
                    clipCounter++;
                    firingTracker = SetTarget(firingCooldown);
                } else if (clipCounter >= clipSize)
                {
                    reloading = true;
                    reloadTracker = SetTarget(reloadTime);
                    myStateMovement.moving = false;
                }
                break;
            case approaching:
                if (OnTarget(searchTarget))
                {
                    // Activate if the enemy isn't behind a wall and within range
                    if (myBase.player != null && myStateMovement.PlayerDistance() <= maxRange && !Physics2D.Linecast(transform.position, myBase.player.transform.position, mask))
                    {
                        SetState(activating);
                        searchTarget = SetTarget(searchCooldown);
                        break;
                    }
                    searchTarget = SetTarget(searchCooldown);
                }
                // Animate moving sprites
                anim.SetInteger("State", 0);
                sprite.flipX = isPlayerLeft();
                anim.SetBool("Mirror", sprite.flipX);

                break;
            default:
                // Animate idle sprites
                anim.SetInteger("State", 2);
                sprite.flipX = isPlayerLeft();
                anim.SetBool("Mirror", sprite.flipX);
                break;
        }

        prevState = current;
    }
}
