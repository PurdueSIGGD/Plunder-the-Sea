using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yPositionLayering : MonoBehaviour
{
    private int startLayer = 0;
    private Transform playerTransform;
    private bool front;
    private SpriteRenderer renderer;
    private float characterHeight = 0.2f;
    private float objectHeight = 0.5f;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        objectHeight = renderer.size.y/2;
        playerTransform = FindObjectOfType<PlayerBase>().transform;
        startLayer = renderer.sortingOrder;
        //use this once the player has a sprite
        //characterHeight = playerTransform.GetComponent<SpriteRenderer>().size.y/2;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - objectHeight <= playerTransform.position.y - characterHeight && !front)
        {
            front = true;
            renderer.sortingOrder = startLayer;
        }
        if (transform.position.y - objectHeight > playerTransform.position.y - characterHeight && front)
        {
            front = false;
            renderer.sortingOrder = -startLayer;
        }
    }
}
