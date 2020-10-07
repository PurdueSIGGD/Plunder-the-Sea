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

    //Fishing Methods
    public int[] getBaitArray()
    {
        return baitTypes;
    }

    public int getBaitAtIndex(int arrayIndex)
    {
        return baitTypes[arrayIndex];
    }

    public void addBait(int arrayIndex)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] + 1;
    }

    public void removeBait(int arrayIndex)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] - 1;
    }

    //Can be used to add bait to any index, and also decrement bait as well
    public void addBait(int arrayIndex, int baitAmount)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] + baitAmount;
    }

    

}
