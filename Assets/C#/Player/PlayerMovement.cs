using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public PlayerBase myBase;

    private float movementSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        myBase = (PlayerBase)GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myBase.myRigid.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, Input.GetAxisRaw("Vertical") * movementSpeed);
    }
}
