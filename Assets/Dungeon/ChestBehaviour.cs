using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    [SerializeField]
    private WeaponFactory.CLASS weaponClass;
    private bool used = false;

    public void SetWeaponClass(WeaponFactory.CLASS weaponClass) {
        this.weaponClass = weaponClass;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        WeaponInventory weaponInv = collider.GetComponent<WeaponInventory>();
        if (weaponInv == null || used) {
            return;
        }

        used = true;
        weaponInv.SetWeapon(weaponClass);
        transform.localScale *= 0.5f;

        PlayerPrefs.SetInt("Chests", PlayerPrefs.GetInt("Chests") + 1);

        this.enabled = false;
    }

}
