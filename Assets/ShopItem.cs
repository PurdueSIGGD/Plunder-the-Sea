using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private GameObject chestPrefab;
    [SerializeField]
    private WeaponFactory.CLASS weaponClass;
    [SerializeField]
    private WeaponTables tables;
    private bool used = false;
    [SerializeField]
    private int baitPrice;
    [SerializeField]
    private int baitType;
    private DescriptionInfo descrInfo;

    [SerializeField]
    private SpritesWeaponClassTable rangedWeaponSpritesTable; 
    [SerializeField]
    private PrefabWeaponClassTable projectilePrefabTable; 

    public void SetShopItem(WeaponFactory.CLASS weaponClass, int baitPrice, int baitType) {
        this.weaponClass = weaponClass;
        this.baitPrice = baitPrice;
        this.baitType = baitType;
        SetDescription();
    }

    public void SetDescription()
    {
        if (descrInfo == null)
        {
            return;
        }
        string descr = tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE ? "Melee" : "Ranged";
        descr += " Damage: ";
        descr += tables.damage.get(weaponClass) + "\n";
        descr += tables.about.getDescr(weaponClass);
        descrInfo.name = tables.about.getName(weaponClass);
        descrInfo.baitCost = baitPrice;
        descrInfo.baitType = baitType;
        descrInfo.description = descr;
    }

    public void OnEnable() {
        descrInfo = GetComponent<DescriptionInfo>();
        SetDescription();
        var newTMPText = "T: " + baitType + " P: " + baitPrice;
        GetComponentInChildren<TMPro.TMP_Text>().text = newTMPText;

        Sprite newSprite = null;
        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE) {
            newSprite = projectilePrefabTable.get(weaponClass).GetComponentInChildren<SpriteRenderer>().sprite;
        } else {
            newSprite = rangedWeaponSpritesTable.get(weaponClass);
        }
        GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerStats playerStats = collider.GetComponent<PlayerStats>();
        if (playerStats == null || used) {
            return;
        }

        if (playerStats.hasEnoughBait(baitPrice, baitType)) {
            this.used = true;
            this.enabled = false;
            Destroy(gameObject);

            PlayerPrefs.SetInt("WeaponBait", PlayerPrefs.GetInt("WeaponBait") + baitPrice);

            playerStats.baitInventory.removeBait(baitType, baitPrice);

            var chestGameObj = Instantiate(chestPrefab, this.transform.position, Quaternion.identity);
            chestGameObj.GetComponent<ChestBehaviour>().SetWeaponClass(this.weaponClass);
        }
    }
}
