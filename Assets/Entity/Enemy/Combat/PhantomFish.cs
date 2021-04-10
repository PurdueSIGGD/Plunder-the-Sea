using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomFish : StateCombat
{
    // Projectile to be used as fire and fake phantom fish
    public GameObject fireProjectile;
    public GameObject fakePhantom;

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

    // Stats for fading back into existence
    private float fadeCooldown = 1.3f;
    private float fadeTarget = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Dragon fish doesn't use its movement very much
        current = GetState();
        prevState = current;

        // Place the fire if it can
        float dist = myStateMovement.PlayerDistance();
        if (dist > minFireDistance && dist < maxFireDistance)
        {
            if (OnTarget(fadeTarget))
            {
                anim.SetInteger("State", 2);
            }
        } else
        {
            if (OnTarget(fadeTarget))
            {
                anim.SetInteger("State", 0);
            }
        }
        anim.SetBool("Back", isPlayerUp());
        sprite.flipX = isPlayerLeft();

        if (OnTarget(fireTarget) && dist > minFireDistance && dist < maxFireDistance)
        {
            fireTarget = SetTarget(fireCooldown);
            Projectile fire = Shoot(fireProjectile);
            //fire.damage = 2 * myBase.myStats.damage;
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
        //if (Vector3.Distance(transform.position, myBase.player.transform.position) <= attackRange)
        {
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage, myBase.myStats);
        }
        myBase.myMovement.moving = true;
    }

    public override void BeforeTeleport()
    {
        Vector3 pos = transform.position;
        Vector3 vel = myBase.myRigid.velocity;
        Vector3 destVector = new Vector3(pos.x + vel.x, pos.y + vel.y, pos.z);
        Projectile fake = Shoot(fakePhantom, transform.position, destVector);
        fake.speed = vel.magnitude;
        fake.transform.rotation = Quaternion.identity;
        fake.transform.localScale = transform.localScale;
        SpriteRenderer fakeSprite = fake.GetComponentInChildren<SpriteRenderer>();
        if (fakeSprite)
        {
            fakeSprite.flipX = sprite.flipX;
            fakeSprite.color = sprite.color;
        }
    }

    public override void AfterTeleport()
    {
        anim.SetInteger("State", 3);
        fadeTarget = SetTarget(fadeCooldown);
    }
}
