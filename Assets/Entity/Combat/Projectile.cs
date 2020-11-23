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
    [HideInInspector]
    public GameObject source;
    [HideInInspector]
    public Weapon weapon;

    public Vector2 direction;

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
            weapon.OnHit(this, ent);
            EntityStats attacker = source.GetComponent<EntityStats>();
            ent.TakeDamage(damage, attacker);
        }

        /* Range proj. always destroy on non-entities */
        if ((!(weapon?.isMelee == true) && !ent) || destroyOnCollide)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if (weapon != null)
        {
            weapon.OnEnd(this);
            Destroy(gameObject);
        }
    }

    public static Projectile Shoot(GameObject prefab, GameObject source, Vector2 target)
    {
        Projectile bullet = Shoot(prefab, source.transform.position, target);
        bullet.source = source;
        return bullet;
    }

    public static Projectile Shoot(GameObject prefab, Vector2 startPos, Vector2 target)
    {

        Projectile bullet = Instantiate(prefab, startPos, Quaternion.identity).GetComponent<Projectile>();
        
        if (!bullet)
        {
            return null;
        }

        Vector2 direction = (target - startPos).normalized;
        bullet.direction = direction;
        

        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(direction.x, direction.y, 0));

        return bullet;

    }
}
