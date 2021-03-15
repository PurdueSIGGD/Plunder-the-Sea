using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAttribute : MonoBehaviour
{
    // Unique script that just gives an entity a permanent attribute
    public EntityStats source;
    public EntityAttribute attr;

    void Start()
    {
        source.AddAttribute(attr, source);
    }
}
