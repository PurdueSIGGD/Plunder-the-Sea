using UnityEngine;

[CreateAssetMenu(fileName = "AmmoProjSys", menuName = "ScriptableObjects/ProjectileSystems/Ammo", order = 1)]
public class AmmoProjSys : ProjectileSystem {

    public override bool CanShoot(GameObject player)
    {
        return weapon.ammo > 0;
    }
    public override void OnFire(Projectile projectile)
    {
        weapon.ammo--;
        if (weapon.ammo < 0)
        {
            weapon.ammo = 0;
        }
    }
    public override void OnKill(EntityStats victim)
    {
        weapon.ammo += weapon.ammoPerKill;
        if (weapon.ammo > weapon.maxAmmo)
        {
            weapon.ammo = weapon.maxAmmo;
        }
    }
    public override void OnEquip(WeaponInventory inv)
    {
        weapon.ammo = weapon.maxAmmo;
    }
}