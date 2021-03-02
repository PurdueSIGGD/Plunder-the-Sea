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

    // How long the gatling squid can fire before reloading


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

        switch (current)
        {
            case cooldown:
                if (OnTarget(searchTarget))
                {
                   // Get closer to the enemy if they're hiding behind a wall
                   if (myBase.player == null || Physics2D.Linecast(transform.position, myBase.player.transform.position, mask))
                    {
                        SetState(approaching);
                        searchTarget = SetTarget(searchCooldown);
                        break;
                    }
                    searchTarget = SetTarget(searchCooldown);
                }
                // Try to shoot if possible and the cooldown allows
                if (OnTarget(firingTracker))
                {
                    Shoot(squidBullet);
                    firingTracker = SetTarget(firingCooldown);
                }
                break;
            case approaching:
                if (OnTarget(searchTarget))
                {
                    // Activate if the enemy isn't behind a wall
                    if (myBase.player != null && !Physics2D.Linecast(transform.position, myBase.player.transform.position, mask))
                    {
                        SetState(activating);
                        searchTarget = SetTarget(searchCooldown);
                        break;
                    }
                    searchTarget = SetTarget(searchCooldown);
                }
                break;
            default:
                break;
        }

        prevState = current;
    }
}
