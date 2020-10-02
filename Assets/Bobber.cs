using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    void Start() 
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        var fish = collider.GetComponent<Fish>();
        if (fish == null)
        {
            return;
        }

        Debug.Log("Start fishing game");
    }
}
