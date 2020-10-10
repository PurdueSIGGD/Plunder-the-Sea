using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENT_ATTR
{
    MOVESPEED,
    MAX_HP,
    MAX_STAMINA,
    TOTAL_STATS
};

public class EntityAttribute
{

    public ENT_ATTR type;
    public float value;
    public float endTime;

    public EntityAttribute(ENT_ATTR type, float value, float duration = float.PositiveInfinity)
    {
        this.type = type;
        this.value = value;
        if (duration != float.PositiveInfinity)
        {
            this.endTime = Time.time + duration;
        }
    }

    public void OnAdd(EntityStats owner)
    {
        PlayerStats player = owner.GetComponent<PlayerStats>();

        /* General entity stats */
        switch(type)
        {
            case ENT_ATTR.MOVESPEED:
                owner.movementSpeed += value;
                break;
            case ENT_ATTR.MAX_HP:
                owner.maxHP += value;
                break;
        }

        /* Player specific stats */
        if (player)
        {
            switch(type)
            {
                case ENT_ATTR.MAX_STAMINA:
                    player.staminaMax += value;
                    break;
            }
        }
    }

    public void OnRemove(EntityStats owner)
    {
        PlayerStats player = owner.GetComponent<PlayerStats>();

        /* General entity stats */
        switch (type)
        {
            case ENT_ATTR.MOVESPEED:
                owner.movementSpeed -= value;
                break;
            case ENT_ATTR.MAX_HP:
                owner.maxHP -= value;
                break;
        }

        /* Player specific stats */
        if (player)
        {
            switch (type)
            {
                case ENT_ATTR.MAX_STAMINA:
                    player.staminaMax -= value;
                    break;
            }
        }
    }

}
