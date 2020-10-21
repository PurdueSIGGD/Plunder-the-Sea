using UnityEngine;
public abstract class ProjectileSystem : ScriptableObject
{
    public virtual bool CanShoot(GameObject player) => true;
    public virtual void Run(Projectile projectile) {}
    public virtual void OnEquip(WeaponInventory inv) {}
    public virtual void OnUnequip(WeaponInventory inv) {}

    public virtual void OnFire(Projectile projectile) {}
    public virtual void OnEnd(Projectile projectile) {}
}
