using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{

    public GameObject weaponPrefab;


    private bool used = false;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        WeaponInventory weaponInv = collider.GetComponent<WeaponInventory>();
        if (weaponInv == null || used) {
            return;
        }

        weaponInv.SetWeaponPrefab(weaponPrefab);
        

        used = true;

        this.enabled = false;
    }

}
