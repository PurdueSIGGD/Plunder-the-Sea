using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    public GameObject targetToSpawn;
    public GameObject target = null;

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            Debug.Log("Spawn");
            target = Instantiate(targetToSpawn, transform.position, Quaternion.identity);
        }
    }
}
