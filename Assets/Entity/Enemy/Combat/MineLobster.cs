using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLobster : StateCombat
{
    // Enemy to be used as venom mines
    public GameObject venomMine;
    // Projectile to be used as venom cloud on death
    public GameObject venomCloudProjectile;

    // Number of seconds between mine
    public float mineCooldown = 2f;

    // Distance from the player that the mine lobster drops venom mines
    public float mineDistance = 3f;

    // The spread distance that mines can be
    public float mineSpread = 0.2f;

    // The current state
    private int current = 0;

    // The target timing for a melee attack (and mines)
    private float meleeTarget = 0.0f;
    private float mineTarget = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Dragon fish doesn't use its movement very much
        current = GetState();
        prevState = current;

        if (myStateMovement.PlayerDistance() < mineDistance)
        {
            anim.SetInteger("State", 1);
        } else
        {
            anim.SetInteger("State", 0);
            anim.SetBool("Back", isPlayerUp());
        }
        sprite.flipX = isPlayerLeft();

        // Place the mine if it can
        if (OnTarget(mineTarget) && myStateMovement.PlayerDistance() < mineDistance)
        {
            mineTarget = SetTarget(mineCooldown);
            PlaceMine(mineSpread);
        }
    }

    // Places a mine with a spread within distance.
    void PlaceMine(float distance)
    {
        Vector3 spreadVector = new Vector3(Random.Range(-distance, distance), Random.Range(-distance, distance), 0);

        Instantiate(venomMine, transform.position + spreadVector, Quaternion.identity).GetComponent<Projectile>();
    }

    // Places some mines on death.
    public override void OnDeath()
    {
        Shoot(venomCloudProjectile);
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
