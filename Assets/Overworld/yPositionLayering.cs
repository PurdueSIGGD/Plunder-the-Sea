using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yPositionLayering : MonoBehaviour
{
    private int startLayer = 0;
    private Transform playerTransform;
    private bool front;
    private SpriteRenderer renderer;
    private float characterHeight = 0.55f;
    private float objectHeight = 0.5f;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        objectHeight = renderer.size.y/2;
        playerTransform = FindObjectOfType<PlayerBase>().transform;
        startLayer = renderer.sortingOrder;
        sortOBJ(true);
    }

    // Update is called once per frame
    void Update()
    {
        sortOBJ(false);
    }

    private void sortOBJ(bool first)
    {
        if (playerTransform)
        {
            if (transform.position.y - objectHeight <= playerTransform.position.y - characterHeight && (!front || first))
            {
                front = true;
                renderer.sortingOrder = startLayer;
            }
            if (transform.position.y - objectHeight > playerTransform.position.y - characterHeight && (front || first))
            {
                front = false;
                renderer.sortingOrder = -startLayer;
            }
        }
        else
        {
            playerTransform = FindObjectOfType<PlayerBase>().transform;
        }
    }
}
