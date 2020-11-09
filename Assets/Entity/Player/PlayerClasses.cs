using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClasses : MonoBehaviour
{
    //modifies the initial player values or abilities
    public float baseHp;
    public float baseSpeed;
    public float meleeDamage;
    public float rangedDamage;
    public WeaponModifiers weaponModifiers;
    private PlayerStats stats;

    public void Awake()
    {
        stats = GetComponent<PlayerStats>();
        stats.maxHP = baseHp;
        stats.movementSpeed = baseSpeed;

        weaponModifiers = new WeaponModifiers();
        weaponModifiers.meleeDamage = meleeDamage;
        weaponModifiers.rangedDamage = rangedDamage;
    }

    //modifies and sent to wepaons on attack
    public struct WeaponModifiers
    {
        public float meleeDamage;
        public float rangedDamage;
    }


}
