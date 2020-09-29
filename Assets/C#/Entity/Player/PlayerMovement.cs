using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public PlayerBase myBase;

    // Start is called before the first frame update
    void Start()
    {
        myBase = (PlayerBase)GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myBase.rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * myBase.stats.movementSpeed, Input.GetAxisRaw("Vertical") * myBase.stats.movementSpeed);
    }
}
