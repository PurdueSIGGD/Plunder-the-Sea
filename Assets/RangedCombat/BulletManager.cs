using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    float lifeTime = 2;
    float currentLifeTime = 0;
    public Vector2 velocity;
    public GameObject origin;

    void Update()
    {
        transform.position += (Vector3) (velocity * Time.deltaTime);

        currentLifeTime += Time.deltaTime;
        if(currentLifeTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
