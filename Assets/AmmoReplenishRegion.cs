using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoReplenishRegion : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider) {

        var stats = collider.GetComponent<PlayerStats>();

        if (stats != null) {
            stats.replenishAmmo(stats.maxAmmo - stats.ammo);
        }
    }
}
