using UnityEngine;
using System.Collections.Generic;

public class WeaponSystem {

    private List<Projectile> projectiles = new List<Projectile>();
    private readonly ProjectileSystem[] projSystems;

    public WeaponSystem(WeaponFactory.CLASS weaponClass, WeaponTables tables) {
        this.projSystems = WeaponFactory.DeriveSystems(weaponClass, tables);
    }

    public void Update() {
        projectiles.RemoveAll((p) => p == null);

        foreach (var sys in projSystems) {
            foreach (var proj in projectiles)
            {
                sys.Run(proj);
            }
        }
    }

    public bool CanShoot(GameObject gameObject) {

        foreach (var sys in projSystems) {
            if (!sys.CanShoot(gameObject)) {
                return false;
            }
        }

        return true;
    }
    /* Called when projectile hits entity */
    public void OnHit(Projectile proj, EntityStats victim)
    {
        foreach (var sys in projSystems)
        {
            sys.OnHit(proj, victim);
        }
    }

    public void OnFire(Projectile projectile) { 
        projectiles.Add(projectile);
        foreach (var sys in projSystems) 
        {
            sys.OnFire(projectile);
        }
    }
    /* Called when projectile dies either by lifetime or hit detection */
    public virtual void OnEnd(Projectile projectile) { 
        foreach (var sys in projSystems)
        {
            sys.OnEnd(projectile);
        }
        projectiles.Remove(projectile);
    }
    /* Called every frame weapon is equipped */
    public void OnEquip(WeaponInventory inv, WeaponTables tables, WeaponFactory.CLASS weaponClass) { 
        foreach (var sys in projSystems) 
        {
            sys.OnEquip(inv, tables, weaponClass);
        }
    }
    public virtual void OnUnequip(WeaponInventory inv) { 
        foreach (var sys in projSystems)
        {
            sys.OnUnequip(inv);
        }
    }

    public virtual void OnKill(EntityStats victim)
    {
        foreach (var sys in projSystems)
        {
            sys.OnKill(victim);
        }
    }

}