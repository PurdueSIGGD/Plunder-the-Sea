using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caveTeleporter : MonoBehaviour
{

    [SerializeField]
    private Vector3 teleportLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = teleportLocation;
        }
    }
}
