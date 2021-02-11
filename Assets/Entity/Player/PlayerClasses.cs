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
    private Rigidbody2D rigid;

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
    public float meleeLungeDistance = 4;
    [Range(0, 16)]
    public float rangedLungeDistance = 8;
    public float lungeOneDirectionCooldown = 0.25f;
    public float lungeBackDirectionCooldown = 0.1f;
    private float lungeCooldownTimer = 0;
    private bool lastLungeMelee = true;

    [Space(10)]

    public bool killChain = false;
    [Range(1, 8)]
    public int killRequirement = 3;
    [Range(0, 32)]
    public float chainTime = 4;
    [Range(1, 8)]
    public float attackSizeBoost = 2;
    [Range(1, 8)]
    public float speedBoost = 2;
    private int kills = 0;
    private float killCountdown = 0;

    [Header("--Starting Weapons (not yet implemented)--")]
    public WeaponFactory.CLASS melee;
    public WeaponFactory.CLASS ranged;
    //this will be implemented after the weapon changes

    //modifies weapon stats
    [Header("--Weapon stat modifiers--")]
    public float meleeDamageAddition = 0;           //implemented
    [Range(0, 8)]
    public float meleeDamageMultiplier = 1;         //implemented
    public float rangedDamageAddition = 0;          //implemented
    [Range(0, 8)]
    public float rangedDamageMultiplier = 1;        //implemented
    public float meleeSizeAddition = 0;     //NOT IMPLEMENTED --------------
    [Range(0, 8)]
    public float meleeSizeMultiplier = 1;   //NOT IMPLEMENTED --------------
    public float rangedSpeedAddition = 0;           //implemented
    [Range(0, 8)]
    public float rangedSpeedMultiplier = 1;         //implemented
    public float projectileLifetimeAddition = 0;    //implemented
    [Range(0, 8)]
    public float projectileLifetimeMultiplier = 1;  //implemented
    [Range(0, 16)]
    public int maxAmmo = 6;                         //implemented

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

            rigid = GetComponent<Rigidbody2D>();
            
            inventory.SetWeapon(melee);
            inventory.SetWeapon(ranged);
        }
        else
        {
            //if not a player

        }
    }

    private void Update()
    {
        lungeCooldownTimer += Time.deltaTime;
        if (lungeCooldownTimer > lungeOneDirectionCooldown)
        {
            lungeCooldownTimer = lungeOneDirectionCooldown;
        }
        
        if (lunge)
        {
            if (Input.GetButtonDown("Fire2") && ((lastLungeMelee == false && lungeCooldownTimer >= lungeBackDirectionCooldown) || (lastLungeMelee == true && lungeCooldownTimer >= lungeOneDirectionCooldown)))
            {
                //forward lunge

                //get looking direction
                Vector3 lookDirection = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);

                rigid.AddForce(lookDirection.normalized * meleeLungeDistance * 2, ForceMode2D.Impulse);
                lastLungeMelee = true;
                lungeCooldownTimer = 0;
                Debug.Log("Lunge foreward");
            } 
            else
            {
                if (Input.GetButtonDown("Fire1") && ((lastLungeMelee == true && lungeCooldownTimer >= lungeBackDirectionCooldown) || (lastLungeMelee == false && lungeCooldownTimer >= lungeOneDirectionCooldown)))
                {
                    //backward lunge

                    //get looking direction
                    Vector3 lookDirection = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);

                    rigid.AddForce(-lookDirection.normalized * meleeLungeDistance * 2, ForceMode2D.Impulse);
                    lastLungeMelee = false;
                    lungeCooldownTimer = 0;
                    Debug.Log("Lunge backward");
                }
            }
        }
        
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
        inventory.SetWeapon(melee);
        inventory.SetWeapon(ranged);
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
        stats.resetAmmo(maxAmmo);
    }

    public void setWeaponMods(WeaponModifiers weaponModifiers)
    {
        weaponModifiers.meleeDamageAddition = meleeDamageAddition;
        weaponModifiers.meleeDamageMultiplier = meleeDamageMultiplier;
        print(meleeDamageMultiplier);
        print(weaponModifiers.meleeDamageMultiplier);
        weaponModifiers.rangedDamageAddition = rangedDamageAddition;
        weaponModifiers.rangedDamageMultiplier = rangedDamageMultiplier;
        weaponModifiers.meleeSizeAddition = meleeSizeAddition;
        weaponModifiers.meleeSizeMultiplier = meleeSizeMultiplier;
        weaponModifiers.rangedSpeedAddition = rangedSpeedAddition;
        weaponModifiers.rangedSpeedMultiplier = rangedSpeedMultiplier;
        weaponModifiers.projectileLifetimeAddition = projectileLifetimeAddition;
        weaponModifiers.projectileLifetimeMultiplier = projectileLifetimeMultiplier;
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
        attackSizeBoost = pc.attackSizeBoost;
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
        Debug.Log("Enemy got hit");

        if (chainLighting && !sendingLighting)
        {
            sendingLighting = true; //stops lighting damage from spawning lighting infinitely

            //only spawn if chance is high enough
            if (Random.Range(0f, 1f) <= chainChance)
            {
                Debug.Log("Lighting should spawn");
                
                //create chain lighting
                EnemyStats next = null;
                float distance = chainRadius;

                current.TakeDamage(lightingDamage, stats);

                for (int i = 0; i < chainLength; i++)
                {
                    foreach (EnemyStats es in FindObjectsOfType<EnemyStats>())
                    {
                        if (current != es)
                        {
                            float distPlaceholder = Vector2.Distance(current.transform.position, es.transform.position);
                            if (distPlaceholder < distance)
                            {
                                next = es;
                                distance = distPlaceholder;
                            }
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
                        Destroy(g, 0.5f);
                        next.TakeDamage(lightingDamage, stats);
                        current = next;
                    }
                }
            }
            sendingLighting = false;
        }
    }

    //gives Struct when called and does checks (call this from weapon when attacking). True is melee and False is ranged
    public void getMods(WeaponModifiers weaponMods)
    {
        weaponMods.meleeDamageAddition = weaponModifiers.meleeDamageAddition;
        weaponMods.meleeDamageMultiplier = weaponModifiers.meleeDamageMultiplier;
        print(weaponModifiers.meleeDamageMultiplier);
        print(weaponMods.meleeDamageMultiplier);
        weaponMods.rangedDamageAddition = weaponModifiers.rangedDamageAddition;
        weaponMods.rangedDamageMultiplier = weaponModifiers.rangedDamageMultiplier;
        weaponMods.meleeSizeAddition = weaponModifiers.meleeSizeAddition;
        weaponMods.meleeSizeMultiplier = weaponModifiers.meleeSizeMultiplier;
        weaponMods.rangedSpeedAddition = weaponModifiers.rangedSpeedAddition;
        weaponMods.rangedSpeedMultiplier = weaponModifiers.rangedSpeedMultiplier;
        weaponMods.projectileLifetimeAddition = weaponModifiers.projectileLifetimeAddition;
        weaponMods.projectileLifetimeMultiplier = weaponModifiers.projectileLifetimeMultiplier;
    }

    //modifies and sent to wepaons on attack
    public class WeaponModifiers
    {
        public float meleeDamageAddition = 0;
        public float meleeDamageMultiplier = 0;
        public float rangedDamageAddition = 0;
        public float rangedDamageMultiplier = 0;
        public float meleeSizeAddition = 0;
        public float meleeSizeMultiplier = 0;
        public float rangedSpeedAddition = 0;
        public float rangedSpeedMultiplier = 0;
        public float projectileLifetimeAddition = 0;
        public float projectileLifetimeMultiplier = 0;
    }


}
