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
    public Sprite classSprite;

    private PlayerStats stats;

    [Header("--Unique modifiers--")]
    public bool chainLighting = false;
    [Range(1, 16)]
    public float lightingDamage = 4f;
    [Range(0, 1)]
    public float chainChance = 0.5f;
    [Range(1, 8)]
    public int chainLength = 4;
    [Range(1, 8)]
    public int chainRadius = 4;
    public GameObject lightingPrefab;
    private bool sendingLighting = false;

    [Space(10)]

    public bool lunge = false;
    [Range(0, 16)]
    public float meleeLungeDistance = 2;
    [Range(0, 16)]
    public float rangedLungeDistance = 4;

    [Space(10)]

    public bool killChain = false;
    [Range(1, 8)]
    public int killRequirement = 3;
    [Range(0, 32)]
    public float chainTime = 4;
    [Range(1, 8)]
    public float attackSpeedBoost = 2;
    [Range(1, 8)]
    public float speedBoost = 2;
    private int kills = 0;
    private float killCountdown = 0;

    [Header("--Starting Weapons (not yet implemented)--")]
    public ScriptableWeapon meleeWeapon;
    public ScriptableWeapon rangedWeapon;
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
            baseSpeed = classes[classNumber].baseSpeed;
            classes[classNumber].setWeaponMods(weaponModifiers);
            setSpecialAttributes(classes[classNumber]);
            if (classSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = classSprite;
            }
            WeaponInventory inventory = GetComponent<WeaponInventory>();
            if (meleeWeapon != null)
            {
                inventory.SetMelee(meleeWeapon);
            }
            if (meleeWeapon != null)
            {
                inventory.SetRanged(rangedWeapon);
            }
        }
        else
        {
            //if not a player

        }
    }

    private void Update()
    {
        if (killChain)
        {
            if (killCountdown <= 0)
            {
                kills = 0;
                killCountdown = 0;
            } else
            {
                killCountdown -= Time.deltaTime;
            }
            //true if on a kill chain
            if (kills >= killRequirement)
            {
                stats.movementSpeed = baseSpeed * speedBoost;
            } else
            {
                stats.movementSpeed = baseSpeed;
            }
        }
    }

    public void changeClass(int i)
    {
        classNumber = i;
        classes[classNumber].setPlayerStats(stats);
        baseSpeed = classes[classNumber].baseSpeed;
        classes[classNumber].setWeaponMods(weaponModifiers);
        setSpecialAttributes(classes[classNumber]);
        if (classSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = classSprite;
        }
        WeaponInventory inventory = GetComponent<WeaponInventory>();
        if (meleeWeapon != null)
        {
            inventory.SetMelee(meleeWeapon);
        }
        if (meleeWeapon != null)
        {
            inventory.SetRanged(rangedWeapon);
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

    public void setSpecialAttributes(PlayerClasses pc)
    {
        chainLighting = pc.chainLighting;
        chainChance = pc.chainChance;
        chainLength = pc.chainLength;
        lightingDamage = pc.lightingDamage;
        chainRadius = pc.chainRadius;
        lightingPrefab = pc.lightingPrefab;

        lunge = pc.lunge;
        meleeLungeDistance = pc.meleeLungeDistance;
        rangedLungeDistance = pc.rangedLungeDistance;

        killChain = pc.killChain;
        killRequirement = pc.killRequirement;
        chainTime = pc.chainTime;
        attackSpeedBoost = pc.attackSpeedBoost;
        speedBoost = pc.speedBoost;
}

    //call this when an enemy is killed
    public void enemyKilled()
    {
        if (killChain)
        {
            kills++;
            killCountdown = chainTime;
        }
    }

    //call this when an enemy is hit/damaged
    public void enemyHit(EnemyStats current)
    {
        if (chainLighting && !sendingLighting)
        {
            sendingLighting = true; //stops lighting damage from spawning lighting infinitely

            //only spawn if chance is high enough
            if (Random.Range(0f, 1f) <= chainChance)
            {
                //create chain lighting
                EnemyStats next = null;
                float distance = Mathf.Infinity;

                current.TakeDamage(lightingDamage, stats);

                for (int i = 0; i < chainLength; i++)
                {
                    foreach (EnemyStats es in FindObjectsOfType<EnemyStats>())
                    {
                        float distPlaceholder = Vector2.Distance(current.transform.position, es.transform.position);
                        if (distPlaceholder < distance)
                        {
                            next = es;
                            distance = distPlaceholder;
                        }
                    }
                    if (next == null)
                    {
                        break;
                    }
                    else
                    {
                        //instantiate lighting and deal damage
                        GameObject g = Instantiate(lightingPrefab, current.transform.position, Quaternion.identity);
                        g.GetComponent<LineRenderer>().SetPositions(new Vector3[] { current.transform.position, next.transform.position });
                        Destroy(g, 0.1f);
                        next.TakeDamage(lightingDamage, stats);
                        current = next;
                    }
                }
            }
            sendingLighting = false;
        }
    }

    //gives Struct when called and does checks (call this from weapon when attacking). True is melee and False is ranged
    public WeaponModifiers attackCall(bool b)
    {
        WeaponModifiers modsToSend = weaponModifiers;
        if (killChain)
        {
            //true if on a kill chain
            if (kills >= killRequirement)
            {
                modsToSend.meleeAttackSpeedMultiplier *= attackSpeedBoost;
            }
        }
        if (lunge)
        {
            if (b)
            {
                GetComponent<PlayerBase>().rigidBody.AddForce(transform.forward * meleeLungeDistance, ForceMode2D.Impulse);
            }
            else
            {
                GetComponent<PlayerBase>().rigidBody.AddForce(-transform.forward * rangedLungeDistance, ForceMode2D.Impulse);
            }
        }

        //return Struct to give info
        return weaponModifiers;
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
