﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSquid : StateCombat
{
    // Sprite references
    public SpriteRenderer sprite;
    public Animator anim;

    // Const values to make coding easier
    const int flanking = (int)FlankMovement.FlankState.flanking;
    const int stationary = (int)FlankMovement.FlankState.stationary;

    // Barrel Attribute modifier
    EntityAttribute barrelDefense = new EntityAttribute(ENT_ATTR.ARMOR_STATIC, 10);

    // Ink Attribute (to inflict on a hit)
    EntityAttribute inkDebuff = new EntityAttribute(ENT_ATTR.ARMOR_STATIC,-1f,2f);

    // How long the barrel squid has to wait before firing
    public float firingCooldown = 0.5f;
    private float firingTracker = 0;

    // Ink Shot projectile
    public GameObject inkShot;

    // Update is called once per frame
    void Update()
    {
        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        anim.SetInteger("State", current);

        switch (current)
        {
            case flanking:
                if (prevState == stationary)
                {
                    // Just started moving, so add defense
                    myBase.myStats.AddAttribute(barrelDefense, myBase.myStats);

                    sprite.flipX = false;
                    sprite.flipY = false;
                }
                break;
            case stationary:

                // Animation
                sprite.flipX = isPlayerLeft();
                sprite.flipY = isPlayerUp();

                // Gameplay stuff
                if (prevState == flanking)
                {
                    // Just stopped moving, so remove defense
                    myBase.myStats.RemoveAttribute(barrelDefense);
                }
                // Try to shoot if possible and the cooldown allows
                if (OnTarget(firingTracker))
                {
                    Shoot(inkShot);
                    firingTracker = SetTarget(firingCooldown);
                } 
                break;
        }
        prevState = current;
    }
}
