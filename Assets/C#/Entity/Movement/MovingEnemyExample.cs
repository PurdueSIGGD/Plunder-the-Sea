using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingEnemyExample : MonoBehaviour
{
    public GameObject playerBody;
    private GeneralMovement moveScript;

    private void Start()
    {
        moveScript = GetComponent<GeneralMovement>();
    }

    //this will move the enemy straight towards the player every physics update stem
    private void FixedUpdate()
    {
        moveScript.sendInput(playerBody);
    }
}
