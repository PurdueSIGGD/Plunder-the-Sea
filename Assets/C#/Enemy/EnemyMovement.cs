using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;
    [HideInInspector]
    public bool moving;

    void Start()
    {
        myBase = GetComponent<EnemyBase>();
    }

    void Update()
    {
        if (moving)
        {
            myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
                * myBase.myStats.movementSpeed;
        }
    }

}
