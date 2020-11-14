using UnityEngine;

[CreateAssetMenu(fileName = "AmmoProjSys", menuName = "ScriptableObjects/ProjectileSystems/Ammo", order = 1)]
public class AmmoProjSys : ProjectileSystem {

    private int maxAmmo;
    public PlayerStats stats;

    public AmmoProjSys(int maxAmmo) {
        this.maxAmmo = maxAmmo;
    }

    public override bool CanShoot(GameObject player)
    {
        return stats.ammo > 0;
    }
    public override void OnFire(Projectile projectile)
    {
        this.stats.decrementAmmo();
    }

    public override void OnEquip(WeaponInventory inv)
    {
        this.stats = inv.GetComponent<PlayerBase>().stats;
        this.stats.resetAmmo(this.maxAmmo);
    }
}