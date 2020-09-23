using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class GeneralMovement : MonoBehaviour
{

    private Rigidbody2D myRigid;

    [SerializeField]
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
    }



    //Use this function to move with a direction
    //This should be called within a FixedUpdate function
    public void sendInput(Vector2 direction)
    {
        myRigid.velocity = direction.normalized * speed;
    }

    //Use this to move with inputs
    //This should be called within a FixedUpdate function
    public void sendInput(float horizontalInput, float verticalInput)
    {
        Vector2 vTemp = new Vector2(horizontalInput, verticalInput);
        myRigid.velocity = vTemp.normalized * speed;
    }

    //Use this to move towards an object
    //This should be called within a FixedUpdate function
    public void sendInput(GameObject g)
    {
        Vector2 vTemp = gameObject.transform.position - transform.position;
        myRigid.velocity = vTemp.normalized * speed;
    }
}
