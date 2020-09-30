using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;
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
            StartCoroutine("meleeAttack");
        }
    }
    IEnumerator meleeAttack() //Executes a melee attack
    {
        yield return new WaitForSeconds(myBase.myStats.attackSpeed);
        Vector2 distance;
        distance = myBase.player.transform.position - this.transform.position;

        if (distance.magnitude <= myBase.myStats.range)
        {
            myBase.player.GetComponent<PlayerBase>().myStats.giveDamage(myBase.myStats.damage);
        }
        myBase.myMovement.moveing = true;
        print("Enemy Attack");
    }

}
