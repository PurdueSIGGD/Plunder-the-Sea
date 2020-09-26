using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    public GameObject projectilePrefab;
    /* Cooldown length in seconds */
    public float projectileCooldown = 0.3f;
    public float timeSinceLastHit;
    public float meleeOffset;

    public bool CanShoot()
    {
        return (Time.time - timeSinceLastHit) >= projectileCooldown;
    }

    /* Returns true if projectile was shot */
    public bool ShootAt(Vector2 position)
    {

        if (CanShoot())
        {
            BulletManager bullet = BulletManager.Shoot(projectilePrefab, gameObject, position, 0f);
            bullet.destroyOnCollide = false;
            bullet.transform.SetParent(this.gameObject.transform);
            timeSinceLastHit = Time.time;
            return true;
        }

        return false;

    }
}
