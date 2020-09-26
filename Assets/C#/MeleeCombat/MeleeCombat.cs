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
        Vector2 projDir = (position - (Vector2)transform.position).normalized;
        if (CanShoot())
        {
            BulletManager bullet = BulletManager.Shoot(projectilePrefab, (Vector2)transform.position + projDir * meleeOffset, position, 0f);
            timeSinceLastHit = Time.time;
            return true;
        }

        return false;

    }
}
