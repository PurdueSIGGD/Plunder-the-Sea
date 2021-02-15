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
    public Image meleeSlot;
    public Image rangeSlot;

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

        weapInv = GetComponentInParent<WeaponInventory>();
        updateWeaponDisplay();
    }

    public void updateWeaponDisplay()
    {
        meleeSlot.sprite = weapInv.getWeaponImage(true);
        Debug.Log(weapInv.getMeleeWeaponClass());
        rangeSlot.sprite = weapInv.getWeaponImage(false);

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
    }

    public void removeBait(int arrayIndex, int baitAmount = 1)
    {
        baitTypes[arrayIndex] = baitTypes[arrayIndex] - baitAmount;
        baitText[arrayIndex].text = "Bait " + (arrayIndex + 1).ToString() + ": " + baitTypes[arrayIndex].ToString();
    }
}
