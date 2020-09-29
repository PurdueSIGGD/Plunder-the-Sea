using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;
    public float attackRange = 0.5f;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
    }

    private void OnTriggerEnter2D(Collider2D collider) //Called when something enters the enemy's range
    {
        if(collider.GetComponent<PlayerBase>())
        {
            myBase.myMovement.moving = false;
            myBase.myRigid.velocity = Vector2.zero;
        }
    }
    IEnumerator meleeAttack() //Executes a melee attack
    {
        yield return new WaitForSeconds(myBase.myStats.attackSpeed);
        if (Vector3.Distance(transform.position, myBase.player.transform.position) <= attackRange)
        {
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage);
        }
        myBase.myMovement.moving = true;
    }
    
}
