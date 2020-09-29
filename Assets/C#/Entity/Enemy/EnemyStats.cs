using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{

    public float attackSpeed = 1;
    public float damage = 1.5f;

    public override void Die()
    {
        Destroy(gameObject);
    }
}
