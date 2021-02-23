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

    [SerializeField]
    private SpritesWeaponClassTable rangedWeaponSpritesTable; 
    [SerializeField]
    private PrefabWeaponClassTable projectilePrefabTable; 

    public void SetShopItem(WeaponFactory.CLASS weaponClass, int baitPrice, int baitType) {
        this.weaponClass = weaponClass;
        this.baitPrice = baitPrice;
        this.baitType = baitType;
    }

    public void OnEnable() {
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

            playerStats.baitInventory.removeBait(baitType, baitPrice);

            var chestGameObj = Instantiate(chestPrefab, this.transform.position, Quaternion.identity);
            chestGameObj.GetComponent<ChestBehaviour>().SetWeaponClass(this.weaponClass);
        }
    }
}
