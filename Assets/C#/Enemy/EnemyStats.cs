using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;

    public float moveSpeed = 1.0f;
    public float maxHealth = 1;
    public float damage = 1;
    public float attackSpeed = 1.0f;
    [HideInInspector]
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

}
