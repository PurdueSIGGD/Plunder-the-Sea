using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    //[HideInInspector]
    public EnemyBase myBase;
    public float attackRange = 0.5f;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
    }
}
