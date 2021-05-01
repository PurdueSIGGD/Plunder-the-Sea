using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishBaitRegion : MonoBehaviour
{
    [SerializeField]
    private int amount;
    void OnTriggerStay2D(Collider2D collider) {
        var stats = collider.GetComponent<PlayerStats>();

        if (stats != null) {
            stats.baitInventory.setAllBaits(amount);
        }
    }
}
