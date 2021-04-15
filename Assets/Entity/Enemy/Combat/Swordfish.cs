using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently very similar to BootTrout, but kept in its own (identical) file so they can be differentiated in the future

public class Swordfish : StateCombat
{
    // Const values to make coding easier
    const int dashing = (int)ChargeMovement.ChargeState.isCharging;
    const int dizzy = (int)ChargeMovement.ChargeState.isRecharging;
    const int charging = (int)ChargeMovement.ChargeState.isChargingUp;
    const int ready = (int)ChargeMovement.ChargeState.ready;

    // The current state
    private int current = 0;

    // The target timing for a melee attack
    private float meleeTarget = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Swordfish is very simple, so no extra AI
        current = GetState();

        // Update variables used by the animator
        anim.SetInteger("State", current);
        if (current == ready)
        {
            // Turn the sprite only while in the normal moving state
            anim.SetBool("Back", isPlayerUp());
            sprite.flipX = isPlayerLeft();
        }

        // Rotate the swordfish once it begins its charge
        if (current == dashing && prevState == charging)
        {
            Vector3 v = ((ChargeMovement)myStateMovement).targetAngle;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            if (sprite.flipX)
            {
                angle += 180;
            }
            sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // De-rotate the swordfish once exits its charge
        if (current == dizzy && prevState == dashing)
        {
            sprite.transform.rotation = Quaternion.identity;
        }
        

        prevState = current;
    }

    private void OnTriggerStay2D(Collider2D collider) //Called when something enters the enemy's range, only activates if charging
    {
        if (current == dashing && collider.GetComponent<PlayerBase>())
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
}
