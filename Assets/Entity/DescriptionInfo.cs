using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionInfo : MonoBehaviour, IPointerEnterHandler
{

    public string name;

    public string buff;

    public bool isFish;

    [HideInInspector]
    public int baitType;

    [HideInInspector]
    public int baitCost = 0;

    [HideInInspector]
    public WeaponFactory.CLASS weaponClass;

    [TextArea]
    public string description;
    private PlayerInventory pInv;
    void Start()
    {
        pInv = FindObjectOfType<PlayerInventory>(true);
    }

    public void updateInventoryDescription()
    {
        if (pInv == null)
        {
            pInv = FindObjectOfType<PlayerInventory>(true);
        }
        if (pInv == null)
        {
            return;
        }
        if (isFish)
        {
            pInv.fishName.text = name;
            pInv.fishImage.sprite = GetComponent<SpriteRenderer>().sprite;
            pInv.fishBait.sprite = pInv.baitImages[GetComponent<Fish>().preferredBaitType];
            pInv.fishDescription.text = description;
            pInv.fishBuffName.text = buff;
            pInv.fishFrame.SetActive(true);
            pInv.weaponFrame.SetActive(false);
        }
        else
        {
            pInv.weaponName.text = name;
            pInv.weaponDescription.text = description;
            if (baitCost > 0)
            {
                pInv.weaponPrice.text = baitCost.ToString();
                pInv.weaponBait.sprite = pInv.baitImages[baitType];
                pInv.weaponBait.color = Color.white;
                pInv.weaponImage.color = Color.clear;
                pInv.weaponX.SetActive(true);
            } else
            {
                pInv.weaponPrice.text = "";
                pInv.weaponBait.color = Color.clear;

                ChestBehaviour CB = GetComponent<ChestBehaviour>();
                Sprite newSprite = null;
                if (CB.weaponTables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE)
                {
                    newSprite = CB.projectilePrefabTable.get(weaponClass).GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    newSprite = CB.rangedWeaponSpritesTable.get(weaponClass);
                }
                pInv.weaponImage.sprite = newSprite;
                pInv.weaponImage.color = Color.white;
                pInv.weaponX.SetActive(false);
            }
            pInv.weaponFrame.SetActive(true);
            pInv.fishFrame.SetActive(false);
        }

        pInv.invCounter = 16;
    }

    // Start is called before the first frame update
    void OnMouseEnter()
    {
        updateInventoryDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        updateInventoryDescription();
    }
}
