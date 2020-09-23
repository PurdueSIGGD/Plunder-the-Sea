using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector]
    public PlayerBase myBase;

    public float movementSpeed = 10.0f;
    public float maxHP = 1;
    public float currentHP = 1;

    // Start is called before the first frame update
    void Start()
    {
        myBase = (PlayerBase)GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
