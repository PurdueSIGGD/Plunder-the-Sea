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
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            print("player is in range");
            StartCoroutine("meleeAttack");
        }
    }
    IEnumerator meleeAttack()
    {
        yield return new WaitForSeconds(myBase.myStats.attackSpeed);
        print("Enemy Attack");
    }
}
