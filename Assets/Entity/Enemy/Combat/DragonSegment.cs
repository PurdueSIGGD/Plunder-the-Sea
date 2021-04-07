using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSegment : MonoBehaviour
{
    // Unique script that just gives an entity a permanent attribute
    public EntityStats source;
    public EntityAttribute attr;

    void Start()
    {
        attr = new EntityAttribute(ENT_ATTR.ARMOR_MULT, 0.75f, float.PositiveInfinity, false, true, "Dragon's Might");
        source.AddAttribute(attr, source);
    }
}
