using UnityEngine;
public abstract class ProjectileSystem : ScriptableObject
{
    protected WeaponTables tables;
    protected WeaponFactory.CLASS weaponClass;


    public virtual bool CanShoot(GameObject player) => true;
    public virtual void Run(Projectile projectile) {}
    public void OnEquip(WeaponInventory inv, WeaponTables tables, WeaponFactory.CLASS weaponClass) {
        this.weaponClass = weaponClass;
        this.tables = tables;
        this.OnEquip(inv);
    }
    protected virtual void OnEquip(WeaponInventory inv) {}
    public virtual void OnUnequip(WeaponInventory inv) {}
    public virtual void OnHit(Projectile proj, EntityStats victim) {}
    public virtual void OnKill(EntityStats victim) {}
    public virtual void OnFire(Projectile projectile) {}
    public virtual void OnEnd(Projectile projectile) {}

}
