using UnityEngine;

[CreateAssetMenu(fileName = "AmmoProjSys", menuName = "ScriptableObjects/ProjectileSystems/Ammo", order = 1)]
public class AmmoProjSys : ProjectileSystem {

    private int maxAmmo;
    public PlayerStats stats;

    protected override void OnEquip(WeaponInventory inv)
    {
        this.maxAmmo = this.tables.maxAmmo.get(weaponClass).Value;
        this.stats = inv.GetComponent<PlayerStats>();
        this.stats.resetAmmo(this.maxAmmo);
    }

    public override bool CanShoot(GameObject player)
    {
        return stats.ammo > 0;
    }
    public override void OnFire(Projectile projectile)
    {
        this.stats.decrementAmmo();
    }

    // only weapons with the "max ammo" stat can use this system
    static AmmoProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.maxAmmo.get(c).HasValue) ? new AmmoProjSys() : null
        );
}