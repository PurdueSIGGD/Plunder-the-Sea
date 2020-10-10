using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityStats : MonoBehaviour
{

    public float movementSpeed = 10.0f;
    public float maxHP = 1;
    public float currentHP = 1;
    public Slider healthbar;

    private List<EntityAttribute> attribs = new List<EntityAttribute>();

    private void Update()
    {
        foreach (EntityAttribute attr in attribs)
        {
            if (Time.time >= attr.endTime)
            {
                RemoveAttribute(attr);
            }
        }
    }

    //Return true if results in death
    public bool TakeDamage(float amount)
    {
        bool died = false;
        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
            died = true;
        }
        if (healthbar != null)
        {
            healthbar.value = currentHP / maxHP;
        }

        return died;

    }

    public void AddAttribute(EntityAttribute attr)
    {
        attribs.Add(attr);
        attr.OnAdd(this);
    }

    public void RemoveAttribute(EntityAttribute attr)
    {
        if (attribs.Remove(attr))
        {
            attr.OnRemove(this);
        }
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
