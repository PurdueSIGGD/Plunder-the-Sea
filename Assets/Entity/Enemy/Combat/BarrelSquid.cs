﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSquid : StateCombat
{
    // Const values to make coding easier
    const int flanking = (int)FlankMovement.FlankState.flanking;
    const int stationary = (int)FlankMovement.FlankState.stationary;
    const int stopping = (int)FlankMovement.FlankState.stopping;
    const int starting = (int)FlankMovement.FlankState.starting;

    // Barrel Attribute modifier
    EntityAttribute barrelDefense = new EntityAttribute(ENT_ATTR.ARMOR_STATIC, 25);

    // How long the barrel squid has to wait before firing
    public float firingCooldown = 0.4f;
    private float firingTracker = 0;

    public float firingDistance = 6f;

    // Ink Shot projectile
    public GameObject inkShot;

    private int current = 0;
    private float meleeTarget = 0;

    // Update is called once per frame
    void Update()
    {
        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        current = GetState();

        anim.SetInteger("State", current);

        switch (current)
        {
            case flanking:
                if (prevState == starting)
                {
                    // Just started moving, so add defense
                    myBase.myStats.AddAttribute(barrelDefense, myBase.myStats);

                }

                // Animation
                //sprite.flipX = isPlayerLeft();
                //sprite.flipY = isPlayerUp();

                if (myBase != null && myBase.myRigid != null && myBase.myRigid.velocity.magnitude >= 0.001f)
                {
                    anim.speed = 1.0f;

                    // Rotate the sprite
                    Vector3 v = myBase.myRigid.velocity;
                    float angle = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg - 90;
                    sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                } else
                {
                    // Don't rotate while not moving
                    anim.speed = 0f;
                }
                

                break;
            case stopping:
                if (prevState == flanking)
                {
                    // Reset animation and rotation
                    sprite.transform.rotation = Quaternion.identity;
                    anim.speed = 1.0f;
                }
                break;
            case stationary:

                // Gameplay stuff
                if (prevState == stopping)
                {
                    // Just stopped moving, so remove defense
                    myBase.myStats.RemoveAttribute(barrelDefense);

                    firingTracker = SetTarget(firingCooldown);
                }
                // Try to shoot if possible and the cooldown allows
                if (OnTarget(firingTracker) && myStateMovement.PlayerDistance() <= firingDistance)
                {
                    Shoot(inkShot, true);
                    firingTracker = SetTarget(firingCooldown);
                } 
                break;
        }
        prevState = current;
    }

    private void OnTriggerStay2D(Collider2D collider) // Melee attack players while in the barrel if elite
    {
        if (myBase.myStats.elite && current == flanking && collider.GetComponent<PlayerBase>())
        {
            if (OnTarget(meleeTarget))
            {
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
    }

    public override void MakeElite(int numEffects)
    {
        base.MakeElite(numEffects);
        FlankMovement fm = ((FlankMovement)myStateMovement);
        fm.minDistance = 0f;
        fm.maxDistance = 0f;
        myBase.myStats.movementSpeed *= 1.5f;
    }
}
