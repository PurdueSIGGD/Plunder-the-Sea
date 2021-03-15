using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomFish : StateCombat
{
    // Projectile to be used as fire
    public GameObject fireProjectile;

    // Number of seconds between fire drops
    public float fireCooldown = 0.5f;

    // Distance from the player that the phantomfish spits fire
    public float maxFireDistance = 8f;
    public float minFireDistance = 1f;

    // The current state
    private int current = 0;

    // The target timing for a melee attack (and fire)
    private float meleeTarget = 0.0f;
    private float fireTarget = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Dragon fish doesn't use its movement very much
        current = GetState();
        prevState = current;

        // Place the fire if it can
        float dist = myStateMovement.PlayerDistance();
        if (OnTarget(fireTarget) && dist > minFireDistance && dist < maxFireDistance)
        {
            fireTarget = SetTarget(fireCooldown);
            Shoot(fireProjectile);
        }
    }

    private void OnTriggerStay2D(Collider2D collider) //Called when something enters the enemy's range
    {
        if (collider.GetComponent<PlayerBase>())
        {
            if (OnTarget(meleeTarget))
            {
                myBase.myMovement.moving = false;
                myBase.myRigid.velocity = Vector2.zero;
                meleeAttack();
                meleeTarget = SetTarget(1.0f / myBase.myStats.attackSpeedInverse);
            }

        }
    }
    void meleeAttack() //Executes a melee attack
    {
        if (Vector3.Distance(transform.position, myBase.player.transform.position) <= attackRange)
        {
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage, myBase.myStats);
        }
        myBase.myMovement.moving = true;
    }
}
