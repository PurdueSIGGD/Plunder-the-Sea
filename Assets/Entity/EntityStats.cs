using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class EntityStats : MonoBehaviour
{

    public float movementSpeed = 10.0f;
    public float maxHP = 1;
    public float currentHP = 1;
    public Slider healthbar;

    /*
     * Storing endtime in attribute class does not allow for the same attribute
     * to be applied to multiple entities. Store in this struct instead.
     */
    private struct AppliedAttribute
    {
        public EntityAttribute attr;
        public float endTime;
    };

    private List<AppliedAttribute> attribList = new List<AppliedAttribute>();

    /* Extending classes must call this in update function */
    protected void StatUpdate()
    {
        for (int i = 0; i < attribList.Count; i++)
        {
            AppliedAttribute app = attribList[i];
            if (Time.time >= app.endTime)
            {
                RemoveAttribute(app.attr);
                i--;//Decrement to account for removed attribute
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
        AppliedAttribute app;
        app.attr = attr;
        if (attr.duration == float.PositiveInfinity)
        {
            app.endTime = float.PositiveInfinity;
        }
        else
        {
            app.endTime = Time.time + attr.duration;
        }
        attribList.Add(app);
        attr.OnAdd(this);
    }

    public void RemoveAttribute(EntityAttribute attr)
    {
        for (int i = 0; i < attribList.Count; i++)
        {
            if (attribList[i].attr == attr)
            {
                attribList.RemoveAt(i);
                attr.OnRemove(this);
            }
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
