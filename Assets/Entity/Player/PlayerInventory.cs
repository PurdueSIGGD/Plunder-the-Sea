using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // Higher Index number means Stronger bait
    public int[] baitTypes = { 0, 0, 0, 0 };
    public Text[] baitText;
    public WeaponInventory weapInv;
    public PlayerBase pBase;
    public Image meleeSlot;
    public Image rangeSlot;
    public Text meleeLabel;
    public Text rangedLabel;
    public Text healthLabel;
    public Text stamLabel;
    public Text ammoLabel;
    public Text speedLabel;
    public Text stamRechLabel;
    public Text meleeDamLabel;
    public Text rangeDamLabel;
    public Text armorLabel;

    //death stats variables
    public int baitsGot = 0;

    private void Start()
    {
        if (baitText.Length != baitTypes.Length)
        {
            Debug.LogError("Make sure there are an identical number of bait types and text objects in each array in the player prefab");
        }

        baitText[0].color = Color.red;
        for (int i = 0; i < baitTypes.Length; i++)
        {
            baitText[i].text = "Bait " + (i + 1).ToString() + ": " + baitTypes[i].ToString();
        }
        pBase = GetComponentInParent<PlayerBase>();
        weapInv = GetComponentInParent<WeaponInventory>();
        updateWeaponDisplay();
        updateStatDisplay();
    }

    void OnEnable()
    {
        updateWeaponDisplay();
        updateStatDisplay();
    }

    public void updateWeaponDisplay()
    {
        if (weapInv != null)
        {
            meleeSlot.sprite = weapInv.getWeaponImage(true);
            meleeLabel.text = "" + weapInv.getMeleeWeaponClass();
            rangeSlot.sprite = weapInv.getWeaponImage(false);
            rangedLabel.text = "" + weapInv.getRangedWeaponClass();
        }
    }

    void Update()
    {
        updateStatDisplay();
    }

    public void updateStatDisplay()
    {
        if (pBase != null)
        {
            healthLabel.text = "HP: " + Mathf.Round(pBase.stats.currentHP) + "/" + Mathf.Round(pBase.stats.maxHP);
            stamLabel.text = "Stam: " + Mathf.Round(pBase.stats.stamina) + "/" + Mathf.Round(pBase.stats.staminaMax);
            ammoLabel.text = "Ammo: " + pBase.stats.ammo + "/" + pBase.stats.maxAmmo;
            armorLabel.text = "Armor: " + pBase.stats.armorStatic + "(x" +pBase.stats.armorMult+ ")";

            speedLabel.text = "Spd: " + pBase.stats.movementSpeed;
            stamRechLabel.text = "Rest: " + pBase.stats.staminaRechargeRate;
            meleeDamLabel.text = "Melee: " + weapInv.projectileDamage(weapInv.getMeleeWeaponClass());
            rangeDamLabel.text = "Range: " + weapInv.projectileDamage(weapInv.getRangedWeaponClass());

            speedLabel.color = Color.white;
            healthLabel.color = Color.white;
            stamLabel.color = Color.white;
            stamRechLabel.color = Color.white;
            meleeDamLabel.color = Color.white;
            rangeDamLabel.color = Color.white;
            armorLabel.color = Color.white;

            if (pBase.stats.appliedStats != null && pBase.stats.appliedStats.Length >= Fish.buffNames.Length)
            {
                speedLabel.color = pBase.stats.appliedStats[0] > 0 ? Color.green : Color.white;
                healthLabel.color = pBase.stats.appliedStats[1] > 0 ? Color.green : Color.white;
                stamLabel.color = pBase.stats.appliedStats[2] > 0 ? Color.green : Color.white;
                stamRechLabel.color = pBase.stats.appliedStats[3] > 0 ? Color.green : Color.white;
                meleeDamLabel.color = pBase.stats.appliedStats[4] > 0 ? Color.green : Color.white;
                rangeDamLabel.color = pBase.stats.appliedStats[5] > 0 ? Color.green : Color.white;
                armorLabel.color = pBase.stats.appliedStats[6] > 0 ? Color.green : Color.white;
            }
        }
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
        baitText[arrayIndex].text = "Bait " + (arrayIndex + 1).ToString() + ": " + baitTypes[arrayIndex].ToString();
        baitsGot += baitAmount;
    }

    public void removeBait(int arrayIndex, int baitAmount = 1)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] - baitAmount;
        baitText[arrayIndex].text = "Bait " + (arrayIndex + 1).ToString() + ": " + baitTypes[arrayIndex].ToString();
    }

    public void flushBait()
    {
        
        for (int i = 0; i < baitTypes.Length; i++)
        {
            baitTypes[i] = 0;
            baitText[i].text = "Bait " + (i + 1).ToString() + ": " + 0.ToString();
        }
        
    }
}
