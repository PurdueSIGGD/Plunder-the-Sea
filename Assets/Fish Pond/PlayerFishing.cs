using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    public GameObject bobberSprite;
    public float castingSpd;
    bool casted = false;
    public bool inWater = false;
    GameObject bobber;

    private void Update()
    {
        if (Input.GetButtonDown("Cast Fishing Pole") && !casted)   // F key
        {
            casted = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            var velocity = ((Vector2)(mousePos - transform.position)).normalized * castingSpd;
            var rb = Instantiate(bobberSprite, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rb.velocity = velocity;
        }
    }
}
