using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5.0f;
    /* Cooldown length in seconds */
    public float projectileCooldown = 0.3f;
    /* Time that cooldown should be over */
    private float projectileCooldownEnd = 0.0f;

    public bool CanShoot()
    {
        return Time.time >= projectileCooldownEnd;
    }

    public void RefreshCooldown()
    {
        projectileCooldownEnd = 0.0f;
    }

    /* Returns true if projectile was shot */
    public bool ShootAt(Vector2 position)
    {

        if (CanShoot())
        {
            Projectile bullet = Projectile.Shoot(projectilePrefab, gameObject, position, projectileSpeed);
            projectileCooldownEnd = Time.time + projectileCooldown;
            return true;
        }

        return false;

    }

}
