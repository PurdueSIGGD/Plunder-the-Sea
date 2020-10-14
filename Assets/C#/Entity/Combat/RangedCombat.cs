using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedCombat : MonoBehaviour
{
    /* Cooldown length in seconds */
    public float projectileCooldown = 0.3f;
    /* Time that cooldown should be over */
    private float projectileCooldownEnd = 0.0f;
    /* Ammo left in the gun */
    public int ammo = 10;
    private int ammoMax = 10;
    private int ammoPerShot = 1;
    public Slider ammoBar;

    public bool CanShoot()
    {
        return Time.time >= projectileCooldownEnd && ammo > 0;
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
            ammo = ammo - ammoPerShot;
            changeAmmoBar();

            var weapon = GetComponent<WeaponInventory>().rangeWeapon;
            var projectilePrefab = weapon.projectilePrefab;
            var projectileSpeed = weapon.initialSpeed;
            Projectile bullet = Projectile.Shoot(projectilePrefab, gameObject, position, projectileSpeed);
            projectileCooldownEnd = Time.time + projectileCooldown;
            return true;
        }

        return false;

    }

    public void addAmmo()
    {
        ammo = Math.Min(ammo + 1, ammoMax);
        changeAmmoBar();
    }

    public void changeAmmoBar() 
    {
        ammoBar.value = (float) ammo / (float) ammoMax;
    }


}
