using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{

    public float movementSpeed = 10.0f;
    public float maxHP = 1;
    public float currentHP = 1;

    //Return true if results in death
    public bool TakeDamage(float amount)
    {

        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
            return true;
        }

        return false;

    }

    public virtual void Die()
    {
        //Do nothing by default
    }

    //When entity is source of a kill
    public virtual void OnKill(EntityStats victim)
    {
        //Do nothing by default
    }

}
