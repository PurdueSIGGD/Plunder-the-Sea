using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClasses : MonoBehaviour
{
    //modifies the initial player values or abilities
    [Header("Player stats/modifiers")]
    public float baseHp = 100;
    public float baseSpeed = 6;
    public float baseArmorAddition = 0;
    public float baseArmorDivision = 0;
    public float baseStamina = 100;
    public float baseStaminaRechargeRate = 2;
    public float proficiency = 1;

    private PlayerStats stats;

    //modifies weapon stats
    [Header("Weapon stats/modifiers")]
    public float meleeDamageAddition = 0;
    public float meleeDamageMultiplier = 1;
    public float rangedDamageAddition = 0;
    public float rangedDamageMultiplier = 1;
    public float critChanceAddition = 0;
    public float critChanceMultiplier = 1;
    public float meleeAttackSpeedAddition = 0;
    public float meleeAttackSpeedMultiplier = 1;
    public float rangedAttackSpeedAddition = 0;
    public float rangedAttackSpeedMultiplier = 1;
    public float projectileSpeedAddition = 0;
    public float projectileSpeedMultiplier = 1;
    public float accuracyAddition = 0;
    public float accuracyMultiplier = 1;
    public float ammoAddition = 0;
    public float ammoMultiplier = 1;

    [Header("Use to acess weapon modifiers")]
    public WeaponModifiers weaponModifiers = new WeaponModifiers();

    public void Awake()
    {
        stats = GetComponent<PlayerStats>();
        stats.maxHP = baseHp;
        stats.movementSpeed = baseSpeed;
        stats.armorStatic = baseArmorAddition;
        stats.armorMult = baseArmorDivision;
        stats.staminaMax = baseStamina;
        stats.staminaRechargeRate = baseStaminaRechargeRate;
        //proficiency is not yet implemented

        weaponModifiers.meleeDamageAddition = meleeDamageAddition;
        weaponModifiers.meleeDamageMultiplier = meleeDamageMultiplier;
        weaponModifiers.rangedDamageAddition = rangedDamageAddition;
        weaponModifiers.rangedDamageMultiplier = rangedDamageMultiplier;
        weaponModifiers.critChanceAddition = critChanceAddition;
        weaponModifiers.critChanceMultiplier = critChanceMultiplier;
        weaponModifiers.meleeAttackSpeedAddition = meleeAttackSpeedAddition;
        weaponModifiers.meleeAttackSpeedMultiplier = meleeAttackSpeedMultiplier;
        weaponModifiers.rangedAttackSpeedAddition = rangedAttackSpeedAddition;
        weaponModifiers.rangedAttackSpeedMultiplier = rangedAttackSpeedMultiplier;
        weaponModifiers.projectileSpeedAddition = projectileSpeedAddition;
        weaponModifiers.projectileSpeedMultiplier = projectileSpeedMultiplier;
        weaponModifiers.accuracyAddition = accuracyAddition;
        weaponModifiers.accuracyMultiplier = accuracyMultiplier;
        weaponModifiers.ammoAddition = ammoAddition;
        weaponModifiers.ammoMultiplier = ammoMultiplier;
    }

    //modifies and sent to wepaons on attack
    public struct WeaponModifiers
    {
        public float meleeDamageAddition;
        public float meleeDamageMultiplier;
        public float rangedDamageAddition;
        public float rangedDamageMultiplier;
        public float critChanceAddition;
        public float critChanceMultiplier;
        public float meleeAttackSpeedAddition;
        public float meleeAttackSpeedMultiplier;
        public float rangedAttackSpeedAddition;
        public float rangedAttackSpeedMultiplier;
        public float projectileSpeedAddition;
        public float projectileSpeedMultiplier;
        public float accuracyAddition;
        public float accuracyMultiplier;
        public float ammoAddition;
        public float ammoMultiplier;
    }


}
