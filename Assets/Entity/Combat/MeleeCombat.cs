using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{

    private PlayerBase pbase;
    private WeaponInventory inventory;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
        inventory = GetComponent<WeaponInventory>();
    }

    public bool CanShoot()
    {
        return pbase.stats.stamina >= inventory.GetMelee().staminaCost;
    }

    /* Returns true if projectile was shot */
    public bool ShootAt(Vector2 position)
    {
        if (CanShoot())
        {
            ScriptableWeapon weapon = inventory.GetMelee();
            Projectile hitbox = Projectile.Shoot(weapon.projectilePrefab, this.gameObject, position, 0f);
            hitbox.weapon = weapon;
            hitbox.damage = weapon.damage;
            hitbox.lifeTime = weapon.lifeTime;
            hitbox.destroyOnCollide = false;
            hitbox.transform.SetParent(this.gameObject.transform);
            pbase.stats.UseStamina(weapon.staminaCost);
            weapon.OnFire(hitbox);
            return true;
        }
        return false;

    }
}
