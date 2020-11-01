using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSquid : StateCombat
{
    // Const values to make coding easier
    const int flanking = (int)FlankMovement.FlankState.flanking;
    const int stationary = (int)FlankMovement.FlankState.stationary;

    // Barrel Attribute modifier


    // Update is called once per frame
    void Update()
    {
        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();
        switch (current)
        {
            case flanking:
                if (prevState == flanking)
                {
                    
                } else
                {

                }
                break;
            case stationary:
                if (prevState == stationary)
                {

                } else
                {

                }
                break;
        }
        prevState = current;
    }
}
