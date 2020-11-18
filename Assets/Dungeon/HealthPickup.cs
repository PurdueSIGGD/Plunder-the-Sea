using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float health = 1.0f;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerStats player = collider.GetComponent<PlayerStats>();
        if (player == null)
        {
            return; //Not a player
        }

        //If player is at full health
        if (player.currentHP == player.maxHP)
        {
            return; //Player at full health
        }

        //Update player health
        player.currentHP = Math.Min(player.currentHP + health, player.maxHP);

        //Destory health pickup
        GameObject.Destroy(transform.gameObject);
    }

}
