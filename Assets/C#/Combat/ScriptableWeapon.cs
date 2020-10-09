using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class ScriptableWeapon : ScriptableObject
{
    public GameObject projectilePrefab;
    public float initialSpeed;
    public int hpDelta;
    public int staminaCost;
    public float lifeTime;
    
    public float maxHpDeltaPercentage;
    public float moveSpeedDeltaPercentage;
}