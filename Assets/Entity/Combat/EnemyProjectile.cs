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

    


    void Update()
    {
        // Operate the projectile
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

        /* Don't collide with the floor */
        if (collider.layer == LayerMask.NameToLayer("Ground"))
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
        if (collider.tag == sourceTag && !friendlyFire)
        {
            return;
        }


        /* Don't collide with other projectiles shot by the same source */
        if (proj && (proj.source == source))
        {
            return;
        }

        var weaponIsMelee = tables.tagWeapon.get(proj.weaponClass) == WeaponFactory.TAG.MELEE;
        /* Deflect with weapons if applicable, which will abort the rest of the collision */
        if (proj && !eproj)
        {
            // If it collides with a weapon projectile that would deflect it
            if ((weaponIsMelee && reflectMelee) || (!weaponIsMelee && reflectRanged))
            {
                Reflect(proj);
                return;
            }
        }

        /* Colliding with an entity */
        EntityStats ent = collider.GetComponent<EntityStats>();
        if (ent)
        {
            // If the collision is with something that uniquely reacts to projectiles
            EnemyCombat ec = GetComponent<EnemyCombat>();
            if (ec)
            {
                if (ec.OnProjectileHit(this)) return;
            }

            // Deal damage
            EntityStats attacker = (source)? source.GetComponent<EntityStats>() : null;
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

    

    public static new EnemyProjectile Shoot(GameObject prefab, GameObject source, Vector2 target, float speed)
    {
        EnemyProjectile bullet = Shoot(prefab, source.transform.position, target, speed);
        bullet.SetSource(source);
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
