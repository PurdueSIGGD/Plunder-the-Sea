﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;
    [HideInInspector]
    public bool moveing;

    // Start is called before the first frame update
    void Start()
    {
        myBase = GetComponent<EnemyBase>();

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveing)
        {
            myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
                * myBase.myStats.moveSpeed;
        }
    }

    //public IEnumerator move() //moves enemy toward player
    //{
        //while (true)
        //{
        //    myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
        //        * myBase.myStats.moveSpeed;
        //    yield return null;
        //}
    //}
}
