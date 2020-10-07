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

    // List of stat modifiers currently applied
    public List<EntityStatModifier> modifiers = new List<EntityStatModifier>();

    //Return true if results in death
    public bool TakeDamage(float amount)
    {

        currentHP -= amount;
        if (healthbar != null)
        {
            healthbar.value = currentHP / maxHP;
        }
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
            return true;
        }

        return false;

    }

    // Modify a stat via a stat modifier, returning the object
    public EntityStatModifier AddModifier(string n, float ms, float mmh, float mch)
    {
        EntityStatModifier mod = gameObject.AddComponent<EntityStatModifier>();

        mod.SetStats(this, n, ms, mmh, mch);

        // Apply changes
        movementSpeed += mod.modSpeed;
        maxHP += mod.modMaxHP;
        currentHP += mod.modCurrentHP;

        // Limit HP to not be higher than max
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        // Add to the current modifiers if not instant
        if (!mod.instant)
        {
            modifiers.Add(mod);
        }

        return mod;
    }

    // Remove a stat modifier currently modifying stats
    // Returns true on a successful remove, returns false on a failure
    // REMOVE DOES NOT CURRENTLY WORK CORRECTLY
    public bool RemoveModifier(EntityStatModifier mod)
    {
        if (modifiers.Contains(mod))
        {
            // Un-modify stats
            movementSpeed -= mod.modSpeed;
            maxHP -= mod.modMaxHP;
            currentHP -= mod.modCurrentHP;

            // Limit HP to not be higher than max
            currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

            modifiers.Remove(mod);
            mod.Remove();

            return true;
        } else
        {
            return false;
        }
    }

    // Remove all modifiers
    // REMOVE DOES NOT CURRENTLY WORK CORRECTLY
    public void RemoveAllModifiers()
    {
        foreach (EntityStatModifier mod in modifiers)
        {
            RemoveModifier(mod);
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
