using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : EntityStats
{
    // Higher Index number means Stronger bait
    private int[] baitTypes = { 0, 0 };

    PlayerBase pbase;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void OnKill(EntityStats victim)
    {
        pbase.OnKill(victim);
    }

    public void addIZeroBait() {
        baitTypes[0] = baitTypes[0] + 1;
    }

    public Array getBaitArray() {
        return baitTypes;
    }
}
