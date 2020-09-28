using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector]
    public EnemyBase myBase;

    public float attackSpeed = 1;
    public float moveSpeed = 1.0f;
    public float maxHealth = 1;
    public float damage = 1.5f;
    public float range = 1;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;

        CircleCollider2D trigger = null;
        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D col in colliders)
        {
            if (col.isTrigger)
            {
                trigger = col;
            }
        }
        trigger.radius = range;
    }
    public void giveDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
