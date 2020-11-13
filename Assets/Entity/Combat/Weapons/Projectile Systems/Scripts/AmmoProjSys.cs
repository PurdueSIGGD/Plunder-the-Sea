using UnityEngine;

[CreateAssetMenu(fileName = "AmmoProjSys", menuName = "ScriptableObjects/ProjectileSystems/Ammo", order = 1)]
public class AmmoProjSys : ProjectileSystem {

    public override bool CanShoot(GameObject player)
    {
        return weapon.stats.ammo > 0;
    }
    public override void OnFire(Projectile projectile)
    {
        weapon.stats.ammo--;
        if (weapon.stats.ammo < 0)
        {
            weapon.stats.ammo = 0;
        }
    }
    public override void OnKill(EntityStats victim)
    {
        weapon.stats.ammo += weapon.stats.ammoPerKill;
        if (weapon.stats.ammo > weapon.stats.maxAmmo)
        {
            weapon.stats.ammo = weapon.stats.maxAmmo;
        }
    }
    public override void OnEquip(WeaponInventory inv)
    {
        weapon.stats.ammo = weapon.stats.maxAmmo;
    }
}