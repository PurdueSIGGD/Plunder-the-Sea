﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : MonoBehaviour
{
    public int numFish = 5;
    public GameObject[] fish;   // collection of all the fish objects
    public GameObject FishingMinigame;

    void Start()
    {
        // spawns random fish from array of fish at the origin of pond
        for (int i = 0; i < numFish; i++)
        {
            int randomFish = Random.Range(0, fish.Length);
            GameObject newFish = Instantiate(fish[randomFish], transform.position, Quaternion.identity);
            newFish.GetComponent<Fish>().FishingMinigame = FishingMinigame;
        }
    }
}
