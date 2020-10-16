using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : EntityStats
{
    // Higher Index number means Stronger bait
    public int[] baitTypes = { 0, 0 };
    public Text[] baitText;

    PlayerBase pbase;
    public const float baseMovementSpeed = 10;
    public const float baseStaminaMax = 100;
    public const float baseStaminaRechargeRate = 2f;
    public const float baseMaxHP = 10;

    public float staminaMax = 100;
    public float stamina = 100;
    public float staminaRechargeRate = 2f;
    public Slider staminaBar;
    private float timeSinceLastTick = 0;
    private float timeBetweenTicks = 0.1f;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();

        for (int i = 0; i < baitTypes.Length; i++)
        {
            baitText[i].text = "Bait " + (i+1).ToString() + ": " + baitTypes[i].ToString();
        }
    }

    private void Update()
    {
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

    //Can be used to add bait to any index, and also decrement bait as well
    public void addBait(int arrayIndex, int baitAmount = 1)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] + baitAmount;
        baitText[arrayIndex].text = "Bait " + (arrayIndex+1).ToString() + ": " + baitTypes[arrayIndex].ToString();
    }

    public void removeBait(int arrayIndex, int baitAmount = 1)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] - baitAmount;
        baitText[arrayIndex].text = "Bait "+ (arrayIndex + 1).ToString()+": "+baitTypes[arrayIndex].ToString();
    }
}
