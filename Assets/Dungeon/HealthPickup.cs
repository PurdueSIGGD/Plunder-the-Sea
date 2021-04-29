using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float health = 1.0f;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private GameObject audioPlayer;
    [SerializeField]
    private AudioClip audio;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerStats player = collider.GetComponent<PlayerStats>();
        if (player == null)
        {
            return; //Not a player
        }

        //If player is at full health
        if (player.currentHP >= player.maxHP)
        {
            return; //Player at full health
        }

        //Update player health
        player.ReplenishHealth(health);

        Instantiate(audioPlayer, transform.position, Quaternion.identity).GetComponent<startAudio>().setup(audio);

        //Destory health pickup
        GameObject.Destroy(transform.gameObject);
    }

}
