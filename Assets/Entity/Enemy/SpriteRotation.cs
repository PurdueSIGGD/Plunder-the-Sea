using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D myRigid;

    public float minMagnitude = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the sprite
        Vector3 v = myRigid.velocity;

        if (myRigid.velocity.magnitude >= minMagnitude)
        {
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 180;
            sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
