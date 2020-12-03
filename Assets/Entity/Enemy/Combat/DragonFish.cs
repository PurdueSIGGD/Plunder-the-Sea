using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFish : StateCombat
{
    // Projectile to be used as fire
    public GameObject fireProjectile;
    
    // Number of seconds between fire drops
    public float fireCooldown = 0.5f;

    // Distance from the player that the dragonfish drops fire
    public float fireDistance = 3f;

    // The spread distance that fire can be (for the normal and on death fire)
    public float fireSpread = 0.2f;
    public float deathSpread = 0.4f;

    // The number of fire particles created when killed.
    public int deathCount = 8;
    
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
        if (OnTarget(fireTarget) && myStateMovement.PlayerDistance() < fireDistance)
        {
            fireTarget = SetTarget(fireCooldown);
            PlaceFire(fireSpread);
        }
    }

    public override void OnDeath()
    {
        for (int i = 0; i < deathCount; i++)
        {
            PlaceFire(deathSpread);
        }
    }

    // Places a fire with a spread within distance.
    void PlaceFire(float distance)
    {
        Vector3 spreadVector = new Vector3(Random.Range(-distance, distance), Random.Range(-distance, distance), 0);

        EnemyProjectile flame = EnemyProjectile.Shoot(fireProjectile, transform.position + spreadVector, transform.position + spreadVector, 0.0f);
        flame.SetSource(gameObject);
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
