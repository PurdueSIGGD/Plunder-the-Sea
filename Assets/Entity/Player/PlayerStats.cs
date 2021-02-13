using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : EntityStats
{
    // Higher Index number means Stronger bait
    public int[] baitTypes = { 0, 0, 0, 0 };
    public Text[] baitText;

    public GameObject healthPickupGameObj;

    PlayerBase pbase;
    WeaponInventory weaponInv;
    public const float baseMovementSpeed = 10;
    public const float baseStaminaMax = 100;
    public const float baseStaminaRechargeRate = 2f;
    public const float baseMaxHP = 10;

    public float staminaMax = 100;
    public float stamina = 100;
    public float staminaRechargeRate = 2f;
    public Slider staminaBar;
    public Slider ammoBar;
    public float killRegen; // goes zero to one
    public Slider killRegenBar;
    public int ammo {get; private set;}
    public int maxAmmo {get; private set;}
    private float timeSinceLastTick = 0;
    private float timeBetweenTicks = 0.1f;

    public void decrementAmmo() {
        this.ammo = Mathf.Max(ammo - 1, 0);
    }

    public void replenishAmmo(int amount) {

        this.ammo = Mathf.Min(this.ammo + amount, maxAmmo);
    }

    public void resetAmmo(int max) {
        this.maxAmmo = max;
        this.ammo = max;
    }

    public void increaseKillRegen(float amount) {
        this.killRegen = Mathf.Min(this.killRegen + amount, 1f);
        if (killRegen == 1f) {
            useKillRegen();
        }
    }

    public void useKillRegen() {
        this.killRegen = 0f;

        var pickupObj = Instantiate(healthPickupGameObj);
        pickupObj.GetComponent<HealthPickup>().health = this.maxHP * 0.25f;

        pickupObj.transform.position = transform.position;
    }


    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
        weaponInv = GetComponent<WeaponInventory>();

        if (baitText.Length != baitTypes.Length)
        {
            Debug.LogError("Make sure there are an identical number of bait types and text objects in each array in the player prefab");
        }

        baitText[0].color = Color.red;
        for (int i = 0; i < baitTypes.Length; i++)
        {
            baitText[i].text = "Bait " + (i+1).ToString() + ": " + baitTypes[i].ToString();
        }
    }

    private void Update()
    {
        StatUpdate();
        staminaBar.value = stamina / staminaMax;
        killRegenBar.value = killRegen;
        if(Time.time > timeSinceLastTick + timeBetweenTicks)
        {
            timeSinceLastTick = Time.time;

            stamina = Mathf.Min(stamina + staminaRechargeRate * timeBetweenTicks, staminaMax);

            ammoBar.value = (float)ammo / (float)maxAmmo;
        }
    }


    public void UseStamina(float staminaCost)
    {
        stamina = Mathf.Max(stamina - staminaCost, 0);
    }

    public override void Die()
    {
        currentHP = maxHP;
        stamina = staminaMax;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void OnKill(EntityStats victim)
    {
        weaponInv?.OnKill(victim);
        pbase.OnKill(victim);
        increaseKillRegen(0.6f);
    }

    //Fishing Methods
    public int[] getBaitArray()
    {
        return baitTypes;
    }

    public void changeRedText(int num)
    {
        for (int i = 0; i < baitTypes.Length; i++)
        {
            if (i == num)
            {
                baitText[i].color = Color.red;
            }
            else
            {
                baitText[i].color = Color.white;
            }
        }
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
