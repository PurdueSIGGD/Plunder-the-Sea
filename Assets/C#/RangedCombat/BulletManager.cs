﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float lifeTime = 2;
    public float currentLifeTime = 0;
    public float damage = 1.0f;
    public bool destroyOnCollide = true;
    public GameObject source;

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if(currentLifeTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        GameObject collider = collision.gameObject;
        /* Don't collide with self */
        if (collider == source)
        {
            return;
        }
        
        EntityStats ent = collider.GetComponent<EntityStats>();
        if (ent)
        {
            ent.TakeDamage(damage);
        }

        if (destroyOnCollide)
        {
            Destroy(gameObject);
        }
    }

    public static BulletManager Shoot(GameObject prefab, GameObject source, Vector2 target, float speed)
    {
        BulletManager bullet = Shoot(prefab, source.transform.position, target, speed);
        bullet.source = source;
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
        Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();

        if (rigidBody)
        {
            rigidBody.velocity = direction * speed;
        }

        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(direction.x, direction.y, 0));

        return bullet;

    }
}
