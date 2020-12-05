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

    // The initial speed of the projectile
    public float speed = 0f;

    // Source's tag (in case the source dies)
    public string sourceTag;

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
            // If the collision is with something that uniquely reacts to projectiles
            EnemyCombat ec = GetComponent<EnemyCombat>();
            if (ec)
            {
                if (ec.OnProjectileHit(this)) return;
            }
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

    // Modifies the trajectory of the bullet to aim towards the source, and modifies this projectile's source (or away from the parameter's source)
    public void Reflect(Projectile proj)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        Vector2 direction;

        if (source)
        {
            // Deflects straight back at the source if it exists
            direction = (source.transform.position - transform.position).normalized;
        }
        else
        {
            // Otherwise, deflects away from the projectile's source
            direction = (transform.position - proj.source.transform.position).normalized;
        }

        if (rigidBody)
        {
            rigidBody.velocity = direction * speed;
        }
        SetSource(proj.source);
    }

    // Used to set the source (and other backup tags in case the source vanishes)
    public void SetSource(GameObject s)
    {
        this.source = s;
        this.sourceTag = s.tag;
    }

    public void Reflect(GameObject deflector)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        Vector2 direction;

        if (source)
        {
            // Deflects straight back at the source if it exists
            direction = (source.transform.position - transform.position).normalized;
        }
        else
        {
            // Otherwise, deflects away from the target
            direction = (transform.position - deflector.transform.position).normalized;
        }

        if (rigidBody)
        {
            rigidBody.velocity = direction * speed;
        }
        SetSource(deflector);
    }

    public static Projectile Shoot(GameObject prefab, GameObject source, Vector2 target, float speed)
    {
        Projectile bullet = Shoot(prefab, source.transform.position, target, speed);
        bullet.SetSource(source);
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
        bullet.speed = speed;

        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(direction.x, direction.y, 0));

        return bullet;

    }
}
