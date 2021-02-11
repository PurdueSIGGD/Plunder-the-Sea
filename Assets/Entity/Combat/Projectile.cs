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
    
    [HideInInspector]
    public int pierceCount = 0;
    //[HideInInspector]
    public GameObject source = null;
    [HideInInspector]
    public WeaponSystem weaponSystem;

    [HideInInspector]
    public WeaponTables tables;
    [HideInInspector]
    public WeaponFactory.CLASS weaponClass;

    [HideInInspector]
    public Vector2 direction;

    // Additional variables to describe how the projectile acts in certain cases
    public bool destroyOnCollide = true;        // The projectile destroys itself when colliding
    public bool destroyOnNonEntity = false;     // The projectile will auto-destroy when colliding with non-entities
    public bool friendlyFire = false;           // The projectile damages those with the same tag
    public bool reflectMelee = false;           // The projectile is reflected by melee attacks
    public bool reflectRanged = false;          // The projectile is reflected by non-melee attacks
    public bool collideWithProjectile = false;  // This projectile collides with other projectiles
    public bool collideWithWall = true;         // This projectile collides with walls

    // Attribute that might be inflicted on a hit, and the chance to hit (between 0.0 and 1.0, a lower number is a lower chance)
    public EntityAttribute attrHit = null;
    public float attrChance = 0.0f;

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
        if (!collideWithProjectile && (proj))
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

        /* Deflect with weapons if applicable, which will abort the rest of the collision */
        if (proj)
        {
            if ((projectileType() == 1 && reflectMelee) || (projectileType() != 1 && reflectRanged))
            {
                Reflect(proj);
                return;
            }
        }

        EntityStats ent = collider.GetComponent<EntityStats>();
        if (ent)
        {
            // If the collision is with something that uniquely reacts to projectiles
            EnemyCombat ec = collider.GetComponent<EnemyCombat>();
            if (ec)
            {
                if (ec.OnProjectileHit(this)) return;
            }
            pierceCount++;
            if (weaponSystem != null)
            {
                weaponSystem.OnHit(this, ent);
            }

            // Deal damage
            EntityStats attacker = (source) ? source.GetComponent<EntityStats>() : null;
            ent.TakeDamage(damage, attacker);

            // Inflict attribute if relevant
            if ((attrHit != null) && (Random.value <= attrChance))
            {
                ent.AddAttribute(attrHit, attacker);
            }
        }

        /* Range proj. always destroy on non-entities */
        //var weaponIsMelee = tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE;
        if ((destroyOnNonEntity && !ent) || destroyOnCollide)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if (weaponSystem != null)
        {
            weaponSystem.OnEnd(this);
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

    // Reflection that doesn't require another projectile (ideal for enemies that can reflect)
    public void Reflect(GameObject deflector)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        Vector2 direction;

        if (source)
        {
            // Deflects straight back at the source if it exists
            direction = (source.transform.position - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x,direction.y,0f));
        }
        else
        {
            // Otherwise, deflects away from the target
            direction = (transform.position - deflector.transform.position).normalized;
        }

        if (rigidBody)
        {
            rigidBody.velocity = direction * rigidBody.velocity.magnitude;
        }
        SetSource(deflector);
    }

    // Function to describe the weapon that this projectile originates from
    // 0 = enemy projectile
    // 1 = melee player weapon
    // 2 = ranged player weapon
    public int projectileType()
    {
        if (tables)
        {
            return (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE) ? 1 : 2;
        } else
        {
            return 0;
        }
    }

    // Used to set the source (and other backup tags in case the source vanishes)
    public void SetSource(GameObject s)
    {
        this.source = s;
        this.sourceTag = s.tag;
    }

    public static Projectile Shoot(GameObject prefab, GameObject source, Vector2 target)
    {
        Projectile bullet = Shoot(prefab, source.transform.position, target);
        bullet.SetSource(source);
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

        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(direction.x, direction.y, 0));

        return bullet;

    }
}
