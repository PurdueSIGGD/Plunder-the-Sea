using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2;
    [HideInInspector]
    public float currentLifeTime = 0;
    public float damage = 1.0f;
    public bool destroyOnCollide = true;
    [HideInInspector]
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
            //If killed by a source
            if (ent.TakeDamage(damage) && source)
            {
                EntityStats attacker = source.GetComponent<EntityStats>();
                if (attacker)
                {
                    attacker.OnKill(ent);
                }
            }
        }

        if (destroyOnCollide)
        {
            Destroy(gameObject);
        }
    }

    public static Projectile Shoot(GameObject prefab, GameObject source, Vector2 target, float speed)
    {
        Projectile bullet = Shoot(prefab, source.transform.position, target, speed);
        bullet.source = source;
        return bullet;
    }

    public static Projectile Shoot(GameObject prefab, Vector2 startPos, Vector2 target, float speed)
    {

        Projectile bullet = Instantiate(prefab, startPos, Quaternion.identity).GetComponent<Projectile>();
        
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
