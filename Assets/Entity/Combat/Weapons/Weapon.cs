using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon {

    public enum CLASS {
        // Melee
        BIG_AXE,
        BLUNDER_BUSS,
        BOTTLE,
        CROSSBOLT,
        CUTLASS,
        DAGGER,
        // Ranged
        DUALIES,
        DUCKSHOT,
        FLINTLOCK,
        GREAT_SWORD,
        HARPOON,
        RAPIER,
        SPEAR,
        SQUIDGUN,
        VOLLEYGUN,
        WHIP
    }

    public static ProjectileSystem MakeMelee(float staminaCost) 
        => new CompositeProjSys( 
            new ProjectileSystem[]{
                new ParentProjSys(),
                new DestroyOnCollideProjSys(false),
                new StaminaProjSys(staminaCost)
            }
        );
    
    public WeaponBaseStats stats;

    public abstract ProjectileSystem[] ConstructSystems();
    public abstract CLASS Class();

    public virtual WeaponBaseStats MakeStats() {
        return new WeaponBaseStats();
    }

    private ProjectileSystem[] projSystems;

    public Weapon() {
        this.projSystems = ConstructSystems();
        this.stats = MakeStats();
    }

    private List<Projectile> projectiles = null;

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
    public void OnEquip(WeaponInventory inv) { 
        foreach (var sys in projSystems) 
        {
            sys.weapon = this;
            sys.OnEquip(inv);
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