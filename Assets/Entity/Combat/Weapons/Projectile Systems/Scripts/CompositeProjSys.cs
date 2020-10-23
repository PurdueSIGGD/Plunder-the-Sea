using UnityEngine;

[CreateAssetMenu(fileName = "CompositeProjSys", menuName = "ScriptableObjects/ProjectileSystems/Composite", order = 1)]
public class CompositeProjSys : ProjectileSystem {

    public ProjectileSystem[] projSystems;
    public override void Run(Projectile projectile) {
        foreach (var sys in projSystems) {
            sys.Run(projectile);
        }
    }

    public override bool CanShoot(GameObject gameObject) {
        foreach (var sys in projSystems) {
            if (!sys.CanShoot(gameObject)) {
                return false;
            }
        }

        return true;
    }

    public override void OnFire(Projectile projectile) { 
        foreach (var sys in projSystems) 
        {
            sys.OnFire(projectile);
        }
    }

    public override void OnEnd(Projectile projectile) { 
        foreach (var sys in projSystems)
        {
            sys.OnEnd(projectile);
        }
    }

    public override void OnEquip(WeaponInventory inv) { 
        foreach (var sys in projSystems) 
        {
            sys.OnEquip(inv);
        }
    }
    public override void OnUnequip(WeaponInventory inv) { 
        foreach (var sys in projSystems)
        {
            sys.OnUnequip(inv);
        }
    }
}