using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletManager : MonoBehaviour
{
    float lifeTime = 2;
    float currentLifeTime = 0;
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if(currentLifeTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public static BulletManager Shoot(GameObject prefab, GameObject source, Vector2 target, float speed)
    {
        BulletManager bullet = Shoot(prefab, source.transform.position, target, speed);
        return bullet;
    }

    public static BulletManager Shoot(GameObject prefab, Vector2 startPos, Vector2 target, float speed)
    {

        BulletManager bullet = Instantiate(prefab, startPos, Quaternion.identity).GetComponent<BulletManager>();
        if (!bullet)
        {
            return null;
        }

        Vector2 direction = (target - startPos).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(direction.x, direction.y, 0));

        return bullet;

    }
}
