﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEnemyStats : EnemyStats
{
    // A simple implementation of a sub-entity, that makes damage it takes and effects it gains redirected to its parent.

    // The parent that all damage will be sent to
    public EntityStats parent;

    public override bool TakeDamage(float amount, EntityStats source, bool tickDamage = false, string killerNameOverride = "")
    {
        // Only adjust incoming damage based on armor
        float realDmg = Mathf.Max((amount - armorStatic), Mathf.Min(1, amount)) * Mathf.Max(1 - armorMult, 0);
        return parent.TakeDamage(realDmg, source);
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
