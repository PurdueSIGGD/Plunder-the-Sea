using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites;            // The sprites to choose from
    public GameObject destination;      // The destination to give the sprite to

    private void Awake()
    {
        destination.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
