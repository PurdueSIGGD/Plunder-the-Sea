﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class ScriptableWeapon : ScriptableObject
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public int damage;
    public float lifeTime;

    [SerializeField]
    private ProjectileSystem[] projSystems = null;

    [HideInInspector]
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