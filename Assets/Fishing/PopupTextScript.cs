using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTextScript : MonoBehaviour
{
    [SerializeField]
    float driftAmount = 0.02f;
    [SerializeField]
    float timeout = 1f;
    float endTime;

    void Start()
    {
        endTime = Time.time + timeout;
    }

    void FixedUpdate()
    {
        if(Time.time > endTime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + driftAmount, transform.position.z);
        }
    }
}
