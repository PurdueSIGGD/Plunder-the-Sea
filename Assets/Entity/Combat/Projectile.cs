using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Lifetime <= 0 is infinite
    public float lifeTime = 2;
    [HideInInspector]
    public float currentLifeTime = 0;
    public float damage = 1.0f;
    public bool destroyOnCollide = true;
    [HideInInspector]
    public int pierceCount = 0;
    //[HideInInspector]
    public GameObject source = null;
    [HideInInspector]
    public ScriptableWeapon weapon;

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if(lifeTime > 0 && currentLifeTime >= lifeTime)
        {
            Destroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        GameObject collider = collision.gameObject;
        Projectile proj = collider.GetComponent<Projectile>();
        /* Don't collide with self OR other projectiles */
        if (collider == source || proj)
        {
            return;
        }
        
        EntityStats ent = collider.GetComponent<EntityStats>();
        if (ent)
        {
            pierceCount++;
            if (weapon)
            {
                weapon.OnHit(this, ent);
            }
            EntityStats attacker = source.GetComponent<EntityStats>();
            ent.TakeDamage(damage, attacker);
        }
        
        /* Range proj. always destroy on non-entities */
        if ((weapon && !weapon.isMelee && !ent) || destroyOnCollide)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if (weapon)
        {
            weapon.OnEnd(this);
        }
        Destroy(gameObject);
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
