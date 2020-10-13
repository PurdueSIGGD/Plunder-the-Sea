using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : EntityStats
{
    // Higher Index number means Stronger bait
    private int[] baitTypes = { 0, 0 };

    PlayerBase pbase;
    public float staminaMax = 100;
    public float stamina = 100;
    public float staminaRechargeRate = 2f;
    public Slider staminaBar;
    private float timeSinceLastTick = 0;
    private float timeBetweenTicks = 0.1f;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
    }

    private void Update()
    {
        StatUpdate();
        staminaBar.value = stamina / staminaMax;
        if(Time.time > timeSinceLastTick + timeBetweenTicks)
        {
            timeSinceLastTick = Time.time;
            stamina = Mathf.Min(stamina + staminaRechargeRate, staminaMax);
        }
    }

    public void UseStamina(float staminaCost)
    {
        stamina = Mathf.Max(stamina - staminaCost, 0);
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
