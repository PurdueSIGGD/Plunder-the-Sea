using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    PlayerBase pbase;

    /* Cooldown length in seconds */
    public float projectileCooldown = 0.3f;
    public float timeSinceLastHit = 0;
    public float meleeOffset;

    public int staminaIndicator;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
    }

    public bool CanShoot()
    {
        return pbase.stats.stamina >= GetComponent<WeaponInventory>().meleeWeapon.staminaCost;
    }

    /* Returns true if projectile was shot */
    public bool ShootAt(Vector2 position)
    {
        if (CanShoot())
        {
            var projectilePrefab = GetComponent<WeaponInventory>().meleeWeapon.projectilePrefab;

            Projectile hitbox = Projectile.Shoot(projectilePrefab, gameObject, position, 0f);
            hitbox.destroyOnCollide = false;
            hitbox.transform.SetParent(this.gameObject.transform);
            timeSinceLastHit = Time.time;
            pbase.stats.UseStamina(GetComponent<WeaponInventory>().meleeWeapon.staminaCost);
            return true;
        }
        return false;

    }
}
