using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparkle : MonoBehaviour
{
    public float lifetime;
    private float killtime;

    private void Start()
    {
        killtime = Time.time + lifetime;
    }

    void Update()
    {
        if (Time.time > killtime)
        {
            DestroyImmediate(this);
        }
    }
}
