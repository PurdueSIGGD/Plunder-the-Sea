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
    
    public float moveSpeedDeltaPercentage;

    public virtual void OnFire(Projectile projectile) { }

}