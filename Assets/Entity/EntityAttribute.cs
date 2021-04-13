using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENT_ATTR
{
    MOVESPEED,      // Changes movespeed by a static amount or multiplier.
    MAX_HP,         // Changes max HP by a static amount or multiplier.
    MAX_STAMINA,    // Changes stamina by a static amount or multiplier.
    ARMOR_STATIC,   // Reduces non-tick damage taken by a static amount, or multiplies an existing value.
    ARMOR_MULT,     // Reduces non-tick damage taken by a multiplier, or multiplies an existing value.
    POISON,         // Deals "value" damage per second to the enemy, ignoring armor.
    TOTAL_STATS,    
    REGEN,          // Heals "value" amount (ratio) of the target's max health per second.
    INVULNERABLE    // The target is immune to damage while this effect is active. When it completes, it removes damage over time debuffs.
};
[System.Serializable]
public class EntityAttribute
{
    public string name;     // Name, used for determining if effects stack (i.e an effect can only stack with a equally-named effect). Leave blank if it isn't relevant (a name of "" won't affect stackability)
    public ENT_ATTR type;   // The type of attribute
    public float value;     // The value of the attribute. What it does depends on the attribute type.
    public float duration;  // The duration of the attibute, in seconds. Zero or less results in infinite.
    public bool stackable;  // Whether the attribute can stack with itself.
    public bool isAdditive; // true: The attribute is additive. false: the attribute is multiplicative. (this is only relevant for modifiers affecting numerical stats, and not for something like poison)

    public EntityAttribute(ENT_ATTR type, float value, float duration = float.PositiveInfinity, bool stackable = false, bool isAdditive = true, string name = "")
    {
        this.name = name;
        this.type = type;
        this.value = value;
        if (duration > 0f)
        {
            this.duration = duration;
        } else
        {
            this.duration = float.PositiveInfinity;
        }
        
        this.stackable = stackable;
        this.isAdditive = isAdditive;
    }

    public void OnAdd(EntityStats owner)
    {
        PlayerStats player = owner.GetComponent<PlayerStats>();

        /* General entity stats */
        switch(type)
        {
            case ENT_ATTR.MOVESPEED:
                if (isAdditive)
                {
                    owner.movementSpeed += value;
                } else
                {
                    owner.movementSpeed *= value;
                }
                
                return;
            case ENT_ATTR.MAX_HP:
                if (isAdditive)
                {
                    owner.maxHP += value;
                }
                else
                {
                    owner.maxHP *= value;
                }
                return;
            case ENT_ATTR.ARMOR_STATIC:
                if (isAdditive)
                {
                    owner.armorStatic += value;
                }
                else
                {
                    owner.armorStatic *= value;
                }
                return;
            case ENT_ATTR.ARMOR_MULT:
                if (isAdditive)
                {
                    owner.armorMult += value;
                }
                else
                {
                    owner.armorMult *= value;
                }
                return;
            case ENT_ATTR.INVULNERABLE:
                owner.invulnerable = true;
                return;
        }

        /* Player specific stats */
        if (player)
        {
            switch(type)
            {
                case ENT_ATTR.MAX_STAMINA:
                    if (isAdditive)
                    {
                        player.staminaMax += value;
                    }
                    else
                    {
                        player.staminaMax *= value;
                    }
                    return;
            }
        }
    }

    public void Update(EntityStats owner, EntityStats source)
    {
        switch(type)
        {
            case ENT_ATTR.POISON:
                owner.TakeDamage(value * Time.deltaTime, source, true, name);
                return;
            case ENT_ATTR.REGEN:
                owner.ReplenishHealth(value * Time.deltaTime * owner.maxHP);
                return;
        }
    }

    public void OnRemove(EntityStats owner)
    {
        PlayerStats player = owner.GetComponent<PlayerStats>();

        /* General entity stats */
        switch (type)
        {
            case ENT_ATTR.MOVESPEED:
                if (isAdditive)
                {
                    owner.movementSpeed -= value;
                }
                else
                {
                    owner.movementSpeed /= value;
                }

                return;
            case ENT_ATTR.MAX_HP:
                if (isAdditive)
                {
                    owner.maxHP -= value;
                }
                else
                {
                    owner.maxHP /= value;
                }
                return;
            case ENT_ATTR.ARMOR_STATIC:
                if (isAdditive)
                {
                    owner.armorStatic -= value;
                }
                else
                {
                    owner.armorStatic /= value;
                }
                return;
            case ENT_ATTR.ARMOR_MULT:
                if (isAdditive)
                {
                    owner.armorMult -= value;
                }
                else
                {
                    owner.armorMult /= value;
                }
                return;
            case ENT_ATTR.INVULNERABLE:
                if (isAdditive)
                {
                    owner.RemoveAttributesByType(ENT_ATTR.POISON);
                }
                owner.invulnerable = false;
                ClassUltimate ults = owner.GetComponent<ClassUltimate>();
                if (ults != null)
                {
                    ults.aura.gameObject.SetActive(false);
                }
                return;
        }

        /* Player specific stats */
        if (player)
        {
            switch (type)
            {
                case ENT_ATTR.MAX_STAMINA:
                    if (isAdditive)
                    {
                        player.staminaMax -= value;
                    }
                    else
                    {
                        player.staminaMax /= value;
                    }
                    return;
            }
        }
    }

}
