using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;
    // Start is called before the first frame update
    void Start()
    {
        myBase = GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collider) //Called when something enters the enemy's range
    {
        if(collider.gameObject.tag == "Player")
        {
            print("player is in range");
            StopCoroutine(myBase.mover);
            myBase.myRigid.velocity = Vector2.zero;
            StartCoroutine(meleeAttack());
        }
    }
    IEnumerator meleeAttack() //Executes a melee attack
    {
        yield return new WaitForSeconds(myBase.myStats.attackSpeed);
        //myBase.player.GetComponent<PlayerBase>().myStats.giveDamage(myBase.myStats.damage);
        print("Enemy Attack");
    }
    
}
