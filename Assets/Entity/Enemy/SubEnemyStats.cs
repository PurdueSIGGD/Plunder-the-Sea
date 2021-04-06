using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEnemyStats : EnemyStats
{
    // A simple implementation of a sub-entity, that makes damage it takes and effects it gains redirected to its parent.

    // The parent that all damage will be sent to
    public EntityStats parent;

    public override bool TakeDamage(float amount, EntityStats source, bool tickDamage = false)
    {
        return parent.TakeDamage(amount, source);
    }

    public override void AddAttribute(EntityAttribute attr, EntityStats source)
    {
        parent.AddAttribute(attr, source);
    }

    public override void RemoveAttribute(EntityAttribute attr)
    {
        parent.RemoveAttribute(attr);
    }

    public override void RemoveAllAttributes()
    {
        parent.RemoveAllAttributes();
    }

    public override void Die()
    {
        parent.Die();
    }

    public override void OnKill(EntityStats victim)
    {
        parent.OnKill(victim);
    }
}
