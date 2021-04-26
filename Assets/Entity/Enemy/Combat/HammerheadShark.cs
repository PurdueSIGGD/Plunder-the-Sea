using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerheadShark : StateCombat
{
    // Const values to make coding easier
    const int spinning = (int)SpinningMovement.SpinningState.spinning;
    const int dizzy = (int)SpinningMovement.SpinningState.dizzy;

    // Spinning Attribute modifier
    public EntityAttribute spinningDefense = new EntityAttribute(ENT_ATTR.ARMOR_MULT, 0.75f, float.PositiveInfinity, false, true, "Spinning");

    // The current state
    private int current = 0;

    // The target timing for a melee attack
    private float meleeTarget = 0.0f;

    // Update is called once per frame
    void Update()
    {
        current = GetState();

        // Elite hammer sharks are always spinning
        if (myBase.myStats.elite)
        {
            myStateMovement.SetState(spinning);
            SpinningMovement sm = ((SpinningMovement)myStateMovement);
            sm.time = 0f;
        }

        switch (current)
        {
            case spinning:
                if (prevState == dizzy)
                {
                    // Just started moving, so add defense
                    if (!myBase.myStats.elite)
                    {
                        myBase.myStats.AddAttribute(spinningDefense, myBase.myStats);
                    }
                    
                    anim.SetInteger("State", 1);
                    anim.SetBool("Back", isPlayerUp());
                    sprite.flipX = isPlayerLeft();
                }
                break;
            case dizzy:
                if (prevState == spinning)
                {
                    // Just stopped moving, so remove defense
                    if (!myBase.myStats.elite)
                    {
                        myBase.myStats.RemoveAttribute(spinningDefense);
                    }

                    anim.SetInteger("State", 0);
                    sprite.flipX = false;
                }
                break;
        }
        prevState = current;
    }

    private void OnTriggerStay2D(Collider2D collider) //Called when something enters the enemy's range, only activates if charging
    {
        if (current == spinning && collider.GetComponent<PlayerBase>())
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

    public override bool OnProjectileHit(Projectile hit)
    {
        // Reflect ranged player projectiles while spinning
        if (current == spinning && hit.ProjectileType() == 2)
        {
            hit.Reflect(gameObject);
            return true;
        }
        return false;
    }

    void meleeAttack() //Executes a melee attack
    {
        //if (Vector3.Distance(transform.position, myBase.player.transform.position) <= attackRange)
        {
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage, myBase.myStats);
        }
        myBase.myMovement.moving = true;
    }

    public override void MakeElite(int numEffects)
    {
        base.MakeElite(numEffects);
        myBase.myStats.movementSpeed *= 0.5f;
        transform.localScale *= 0.75f;
    }
}
