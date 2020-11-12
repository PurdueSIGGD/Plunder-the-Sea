﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootTrout : StateCombat
{
    // Const values to make coding easier
    const int charging = (int)ChargeMovement.ChargeState.isCharging;

    // The current state
    private int current = 0;

    // The target timing for a melee attack
    private float meleeTarget = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Boot Trout is very simple, so no extra AI
        current = GetState();
        prevState = current;
    }

    private void OnTriggerEnter2D(Collider2D collider) //Called when something enters the enemy's range, only activates if charging
    {
        if (current == charging && collider.GetComponent<PlayerBase>())
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
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage);
        }
        myBase.myMovement.moving = true;
    }
}