using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatModifier : MonoBehaviour
{
    // The target stats that are being modified
    public EntityStats parent;

    // A name that can describe the modifier and its source
    public string modifierName;

    // Modifiers to add directly to the multiple stats present in their EntityStats
    public float modSpeed = 0.0f;
    public float modMaxHP = 0.0f;
    public float modCurrentHP = 0.0f;

    // Boolean if the modifier has a duration, and isn't instant (healing would be instant and doesn't stay)
    public bool instant = false;

    // Basic entity stat modifier construction
    public void SetStats(EntityStats p, string n, float ms, float mmh, float mch)
    {
        parent = p;
        modifierName = n;
        modSpeed = ms;
        modMaxHP = mmh;
        modCurrentHP = mch;
    }

    public void Remove()
    {
        DestroyImmediate(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
