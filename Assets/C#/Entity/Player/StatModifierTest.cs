using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierTest : MonoBehaviour
{
    PlayerStats pstats;

    // Start is called before the first frame update
    void Start()
    {
        pstats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Buff();
        }
    }

    // Stat to add stat modifiers
    void Buff()
    {
        pstats.AddModifier("test speed", 10f, 0f, 0f);
    }
}
