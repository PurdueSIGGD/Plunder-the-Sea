using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCombat : EnemyCombat
{
    // Script used for default enemy's combat
    private void OnTriggerEnter2D(Collider2D collider) //Called when something enters the enemy's range
    {
        if (collider.GetComponent<PlayerBase>())
        {
            myBase.myMovement.moving = false;
            myBase.myRigid.velocity = Vector2.zero;
            StartCoroutine("meleeAttack");
        }
    }
    IEnumerator meleeAttack() //Executes a melee attack
    {
        yield return new WaitForSeconds(1 / myBase.myStats.attackSpeedInverse);
        if (Vector3.Distance(transform.position, myBase.player.transform.position) <= attackRange)
        {
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage, myBase.myStats);
        }
        myBase.myMovement.moving = true;
    }
}
