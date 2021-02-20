using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private GameObject chestPrefab;
    [SerializeField]
    private WeaponFactory.CLASS weaponClass;
    private bool used = false;
    [SerializeField]
    private int baitPrice;
    [SerializeField]
    private int baitType;

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
