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

    public virtual void OnFire(Projectile projectile) { }
    /* Called when projectile dies either by lifetime or hit detection */
    public virtual void OnEnd(Projectile projectile) { }
    /* Called every frame weapon is equipped */
    public virtual void Update() { }
    public virtual void OnEquip(WeaponInventory inv) { }
    public virtual void OnUnequip(WeaponInventory inv) { }

}