using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public PlayerBase myBase;

    void Start()
    {
        myBase = (PlayerBase)GetComponent<PlayerBase>();
    }

    void Update()
    {
        float speed = myBase.stats.movementSpeed;
        myBase.rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
    }
}
