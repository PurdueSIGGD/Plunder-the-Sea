using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public ScriptableWeapon weapon;

    public bool isMelee;

    private bool used = false;

    public void OnTriggerEnter2D(Collider2D collider) {
        
        var weaponInv = collider.GetComponent<WeaponInventory>();
        if (weaponInv == null || used) {
            return;
        }

        if (isMelee) {
            weaponInv.meleeWeapon = this.weapon;
        } 
        else {
            weaponInv.rangeWeapon = this.weapon;
        }

        used = true;

        this.enabled = false;
    }

}
