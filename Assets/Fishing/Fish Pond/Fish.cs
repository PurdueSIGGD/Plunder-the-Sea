﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;
    public float randomX;
    public float randomY;
    public float minWaitTime;
    public float maxWaitTime;
    private Vector3 randomPos;
    public float buffMovementSpeed = 0f;
    public float buffMaxStamina = 0f;
    public float buffMaxHP = 0f;
    public float buffStaminaRechargeRate = 0f;
    public int lootLevel;
    public GameObject FishingMinigame;

    void Start()
    {
        RandomPosition();
    }

    public void BuffPlayerStats(PlayerStats stats)
    {
        stats.movementSpeed += buffMovementSpeed;
        stats.maxHP += buffMaxHP;
        stats.staminaMax += buffMaxStamina;
        stats.staminaRechargeRate += buffStaminaRechargeRate;
    }

    // finds random position for fish
    void RandomPosition()
    {
        randomPos = new Vector3(Random.Range(-randomX, randomX), Random.Range(-randomY, randomY), 0);
        StartCoroutine(MoveToRandomPos());
    }

    // moves the fish to the random location from the current position
    IEnumerator MoveToRandomPos()
    {
        float i = 0.0f;
        Vector3 currentPos = transform.position;

        // moves fish to random location
        while (i < 1.0f)
        {
            i += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(currentPos, randomPos, i);
            yield return null;
        }

        float randomFloat = Random.Range(0.0f, 1.0f);
        if (randomFloat < 0.5f)
            StartCoroutine(WaitBetweenMovements());
        else
            RandomPosition();
    }

    // fish waits at position if called then chooses another random location
    IEnumerator WaitBetweenMovements()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        RandomPosition();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if it hits a wall then stop and go the other direction
        if(col.gameObject.tag == "Pond")
        {
            StopAllCoroutines();
            randomPos = -randomPos/2 + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            StartCoroutine(MoveToRandomPos());
        }
    }
}