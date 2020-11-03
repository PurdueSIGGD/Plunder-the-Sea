using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClasses : MonoBehaviour
{
    public float attackMultiplier = 1f;
    public float speedModifier = 0f;

    private void Start()
    {
        GetComponent<PlayerStats>().movementSpeed += speedModifier;
    }

    public struct weaponMods{
        //code here
    }

}
