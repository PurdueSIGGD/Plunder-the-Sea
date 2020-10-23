using UnityEngine;

[CreateAssetMenu(fileName = "AmmoProjSys", menuName = "ScriptableObjects/ProjectileSystems/Ammo", order = 1)]
public class AmmoProjSys : ProjectileSystem {

    public int ammo = 0;

    public override bool CanShoot(GameObject player)
    {
        return ammo > 0;
    }
    public override void OnFire(Projectile projectile)
    {
        ammo--;
        if (ammo < 0)
        {
            ammo = 0;
        }
    }
    public override void OnKill(EntityStats victim)
    {
        ammo += weapon.ammoPerKill;
        if (ammo > weapon.maxAmmo)
        {
            ammo = weapon.maxAmmo;
        }
    }
    public override void OnEquip(WeaponInventory inv)
    {
        ammo = weapon.maxAmmo;
    }
}