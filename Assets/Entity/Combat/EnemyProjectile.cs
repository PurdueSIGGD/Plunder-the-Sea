using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    // Stats describing the projectile (that wouldn't be as relevant for player projectiles)
    public bool friendlyFire = false;
    public bool reflectMelee = false;
    public bool reflectRanged = false;
    public bool collideWithProjectile = false;
    public bool collideWithWall = true;

    // Attribute that might be inflicted on a hit, and the chance to hit (between 0.0 and 1.0, a lower number is a lower chance)
    public EntityAttribute attrHit = null;
    public float attrChance = 0.0f;

    // The initial speed of the projectile
    public float speed = 0f;

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (lifeTime > 0 && currentLifeTime >= lifeTime)
        {
            Destroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        Projectile proj = collider.GetComponent<Projectile>();
        EnemyProjectile eproj = collider.GetComponent<EnemyProjectile>();

        /* Don't collide with source */
        if (collider == source)
        {
            return;
        }

        /* Do / Don't collide with other projectiles */
        if (!collideWithProjectile && (proj || eproj))
        {
            return;
        }

        /* Do / Don't collide with walls */
        if (!collideWithWall && collider.tag == "Wall")
        {
            return;
        }

        /* Do / Don't collide with things w/ the same tag as source */
        if (collider.tag == source.tag && !friendlyFire)
        {
            return;
        }

        /* Don't collide with other projectiles shot by the same source */
        if (proj && (proj.source == source))
        {
            return;
        }

        /* Deflect with weapons if applicable, which will abort the rest of the collision */
        if (proj && !eproj && proj.weapon)
        {
            // If it collides with a weapon projectile that would deflect it
            if ((proj.weapon.isMelee && reflectMelee) || (!proj.weapon.isMelee && reflectRanged))
            {
                Reflect(proj);
                return;
            }
        }

        /* Colliding with an entity */
        EntityStats ent = collider.GetComponent<EntityStats>();
        if (ent)
        {
            // Deal damage
            EntityStats attacker = source.GetComponent<EntityStats>();
            ent.TakeDamage(damage, attacker);

            // Inflict attribut if relevant
            if ((attrHit != null) && (Random.value <= attrChance))
            {
                ent.AddAttribute(attrHit, attacker);
            }
        }

        /* Range proj. always destroy on non-entities */
        if (destroyOnCollide)
        {
            Destroy();
        }
    }

    // Destroys the projectile, which is simple without a weapon
    public void Destroy()
    {
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
        } else
        {
            // Otherwise, deflects away from the projectile's source
            direction = (transform.position - proj.source.transform.position).normalized;
        }

        if (rigidBody)
        {
            rigidBody.velocity = direction * speed;
        }
        source = proj.source;
    }

    public static new EnemyProjectile Shoot(GameObject prefab, GameObject source, Vector2 target, float speed)
    {
        EnemyProjectile bullet = Shoot(prefab, source.transform.position, target, speed);
        bullet.source = source;
        return bullet;
    }

    public static new EnemyProjectile Shoot(GameObject prefab, Vector2 startPos, Vector2 target, float speed)
    {
        EnemyProjectile bullet = Instantiate(prefab, startPos, Quaternion.identity).GetComponent<EnemyProjectile>();

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
