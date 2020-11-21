using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENT_ATTR
{
    MOVESPEED,
    MAX_HP,
    MAX_STAMINA,
    ARMOR_STATIC,
    ARMOR_MULT,
    POISON,
    TOTAL_STATS
};
[System.Serializable]
public class EntityAttribute
{
    
    public ENT_ATTR type;   // The type of attribute
    public float value;     // The value of the attribute. What it does depends on the attribute type.
    public float duration;  // The duration of the attibute, in seconds.
    public bool stackable;  // Whether the attribute can stack with itself.
    public bool isAdditive; // true: The attribute is additive. false: the attribute is multiplicative.

    public EntityAttribute(ENT_ATTR type, float value, float duration = float.PositiveInfinity, bool stackable = false, bool isAdditive = true)
    {
        this.type = type;
        this.value = value;
        this.duration = duration;
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
                owner.TakeDamage(value * Time.deltaTime, source);
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
