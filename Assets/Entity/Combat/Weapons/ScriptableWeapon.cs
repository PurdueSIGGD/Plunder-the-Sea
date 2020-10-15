using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class ScriptableWeapon : ScriptableObject
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public int damage;
    public int staminaCost;
    public float lifeTime;

    [SerializeField]
    private ProjectileSystem[] projSystems;

    [HideInInspector]
    private List<Projectile> projectiles;

    public void Update() {
        projectiles.RemoveAll((p) => p == null);

        foreach (var sys in projSystems) {
            foreach (var proj in projectiles)
            {
                sys.Run(proj);
            }
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
            sys.OnEquip(inv);
        }
    }
    public virtual void OnUnequip(WeaponInventory inv) { 
        foreach (var sys in projSystems)
        {
            sys.OnUnequip(inv);
        }
    }

}