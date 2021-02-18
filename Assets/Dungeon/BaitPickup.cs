using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitPickup : MonoBehaviour
{
    public int baitType = 0;
    public int baitAmount = 1;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerStats player = collider.GetComponent<PlayerStats>();
        if (player == null)
        {
            return; //Not a player
        }

        //Update player bait
        player.baitInventory.addBait(baitType, baitAmount);

        //Destory bait pickup
        GameObject.Destroy(transform.gameObject);
    }

}

