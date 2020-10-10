using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : MonoBehaviour
{
    public float cooldownLength = 0.3f;
    private float cooldownEnd = 0.0f;
    private WeaponInventory inventory;

    private void Start()
    {
        inventory = GetComponent<WeaponInventory>();
    }

    public bool CanShoot()
    {
        return Time.time >= cooldownEnd;
    }

    public void RefreshCooldown()
    {
        cooldownEnd = 0.0f;
    }

    /* Returns true if projectile was shot */
    public bool ShootAt(Vector2 position)
    {
        if (CanShoot())
        {
            ScriptableWeapon weapon = inventory.GetRanged();
            Projectile bullet = Projectile.Shoot(weapon.projectilePrefab, this.gameObject, position, weapon.projectileSpeed);
            bullet.damage = weapon.damage;
            bullet.lifeTime = weapon.lifeTime;
            cooldownEnd = Time.time + cooldownLength;
            weapon.OnFire(bullet);
            return true;
        }

        return false;

    }

}
