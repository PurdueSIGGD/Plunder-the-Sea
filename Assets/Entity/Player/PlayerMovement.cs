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
        
        //I know this is jankey, i may fix it later -Andy
        
        float speed = myBase.stats.movementSpeed;
        Vector2 newVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed * Time.deltaTime;

        myBase.rigidBody.AddForce(newVelocity * 64);
    }
}
