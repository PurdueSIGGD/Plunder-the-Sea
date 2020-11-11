using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClasses : MonoBehaviour
{
    /**
     * This script will be added to the player object, but relies on prefab instances of this class added
     * to the classes array to be able to use stored classes
     * 
     * Scripts not on the player (stored in profabs) should just store the numberical values
     * 
     **/
    
    //used to set the player's class
    [Header("--Classlist stuff (only modify if script is on player)--")]
    public int classNumber = -1;
    public PlayerClasses[] classes;

    //modifies the initial player values or abilities
    [Header("--Player stats--")]
    [Range(1, 64)]
    public float baseHp = 10;
    [Range(0, 32)]
    public float baseSpeed = 6;
    public float baseArmorAddition = 0;
    public float baseArmorDivision = 0;
    [Range(0, 512)]
    public float baseStamina = 100;
    [Range(0, 16)]
    public float baseStaminaRechargeRate = 2;
    [Range(0, 16)]
    public float proficiency = 1;

    private PlayerStats stats;

    [Header("--Starting Weapons (not yet implemented)--")]
    public ScriptableWeapon sw;
    //this will be implemented after the weapon changes

    //modifies weapon stats
    [Header("--Weapon stat modifiers--")]
    public float meleeDamageAddition = 0;
    [Range(0, 8)]
    public float meleeDamageMultiplier = 1;
    public float rangedDamageAddition = 0;
    [Range(0, 8)]
    public float rangedDamageMultiplier = 1;
    public float critChanceAddition = 0;
    [Range(0, 8)]
    public float critChanceMultiplier = 1;
    public float meleeAttackSpeedAddition = 0;
    [Range(0, 8)]
    public float meleeAttackSpeedMultiplier = 1;
    public float rangedAttackSpeedAddition = 0;
    [Range(0, 8)]
    public float rangedAttackSpeedMultiplier = 1;
    public float projectileSpeedAddition = 0;
    [Range(0, 8)]
    public float projectileSpeedMultiplier = 1;
    public float accuracyAddition = 0;
    [Range(0, 8)]
    public float accuracyMultiplier = 1;
    public float ammoAddition = 0;
    [Range(0, 8)]
    public float ammoMultiplier = 1;

    //Use to acess weapon modifiers
    public WeaponModifiers weaponModifiers = new WeaponModifiers();

    public void Awake()
    {
        if (classNumber != -1)
        {
            //if on a player: set this script's weapon struct and the player's stats based on the selected class (does not modify the variables of this script)
            stats = GetComponent<PlayerStats>();
            classes[classNumber].setPlayerStats(stats);
            classes[classNumber].setWeaponMods(weaponModifiers);
        }
        else
        {
            //if not a player

        }
    }

    public void setPlayerStats(PlayerStats stats)
    {
        stats.maxHP = baseHp;
        stats.currentHP = baseHp;
        stats.movementSpeed = baseSpeed;
        stats.armorStatic = baseArmorAddition;
        stats.armorMult = baseArmorDivision;
        stats.staminaMax = baseStamina;
        stats.stamina = baseStamina;
        stats.staminaRechargeRate = baseStaminaRechargeRate;
        //proficiency is not yet implemented
    }

    public void setWeaponMods(WeaponModifiers weaponModifiers)
    {
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
