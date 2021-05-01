using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAmmoRegion : MonoBehaviour
{
    [SerializeField]
    private int amount;
    void OnTriggerEnter2D(Collider2D collider) {
        var stats = collider.GetComponent<PlayerStats>();

        if (stats != null) {
            stats.replenishAmmo(amount - stats.ammo);
        }
    }

}
