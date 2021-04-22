using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : EntityStats
{

    public PlayerInventory baitInventory;
    public float[] appliedStats;
    public GameObject healthPickupGameObj;

    PlayerBase pbase;
    [HideInInspector]
    public WeaponInventory weaponInv;
    public const float baseMovementSpeed = 10;
    public const float baseStaminaMax = 100;
    public const float baseStaminaRechargeRate = 2f;
    public const float baseMaxHP = 10;

    public int dungeonLevel;

    public float staminaMax = 100;
    public float stamina = 100;
    public float staminaRechargeRate = 2f;
    public Slider staminaBar;
    public Slider ammoBar;
    public float killRegen; // goes zero to one
    public Slider killRegenBar;
    public int ammo {get; private set;}
    public int maxAmmo {get; private set;}
    private float timeSinceLastTick = 0;
    private float timeBetweenTicks = 0.1f;

    public float ammoUseChance = 1f; // 0 means no ammo usage
    public float killRegenHealMult = 1f; // Number kill regen healing is multiplied by, capped at 2
    public float projectileSpeedMult = 1f; // Number projectile speed is multiplied by

    //death stats variables
    private float levelTime;

    public void decrementAmmo() {
        this.ammo = Mathf.Max(ammo - 1, 0);
    }

    public void replenishAmmo(int amount) {

        this.ammo = Mathf.Min(this.ammo + amount, maxAmmo);
    }

    public void resetAmmo(int max) {
        this.maxAmmo = max;
        this.ammo = max;
    }

    public void increaseKillRegen(float amount) {
        this.killRegen = Mathf.Min(this.killRegen + amount, 1f);
        if (killRegen == 1f) {
            useKillRegen();
        }
    }

    public bool hasEnoughBait(int baitAmount, int baitType) {
        return this.baitInventory.baitTypes[baitType] >= baitAmount;
    }

    public void useKillRegen() {
        this.killRegen = 0f;

        var pickupObj = Instantiate(healthPickupGameObj);
        pickupObj.GetComponent<HealthPickup>().health = this.maxHP * 0.25f * killRegenMultCalc(this.killRegenHealMult);

        pickupObj.transform.position = transform.position;
    }

    public float killRegenMultCalc(float input)
    {
        return 2f * (input / (1f + input));
    }


    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
        weaponInv = GetComponent<WeaponInventory>();
        baitInventory = GetComponentInChildren(typeof(PlayerInventory), true) as PlayerInventory;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            baitInventory.addBait(0, 256);
            baitInventory.addBait(1, 256);
            baitInventory.addBait(2, 256);
            baitInventory.addBait(3, 256);
        }
        appliedStats = null;
    }

    private void Update()
    {
        StatUpdate();
        staminaBar.value = stamina / staminaMax;
        killRegenBar.value = killRegen;
        if(Time.time > timeSinceLastTick + timeBetweenTicks)
        {
            timeSinceLastTick = Time.time;

            stamina = Mathf.Min(stamina + staminaRechargeRate * timeBetweenTicks, staminaMax);

            ammoBar.value = (float)ammo / (float)maxAmmo;
        }
        levelTime += Time.deltaTime;
    }


    public void UseStamina(float staminaCost)
    {
        stamina = Mathf.Max(stamina - staminaCost, 0);
    }

    private void setDeathStats()
    {
        PlayerPrefs.SetInt("Level", dungeonLevel + 1);
        PlayerPrefs.SetFloat("Time", levelTime);

        PlayerClasses pc = GetComponent<PlayerClasses>();
        PlayerPrefs.SetString("Class", GetComponent<PlayerClasses>().classes[pc.classNumber].name);

        PlayerPrefs.SetInt("BaitGot", baitInventory.baitsGot);
        PlayerPrefs.SetInt("Weapons", weaponInv.wepsGot);

        PlayerPrefs.SetInt("BaitLeft", baitInventory.getBaitTotal());

        //The other playerpref values are set elsewere realtime
    }

    public override void Die()
    {
        currentHP = maxHP;
        stamina = staminaMax;
        ammo = maxAmmo;
        if (appliedStats != null)
        {
            Fish.UnbuffPlayerStats(pbase, false);
        }
        PlayerClasses pClass = GetComponent<PlayerClasses>();
        pClass.initialize();
        setDeathStats();
        baitInventory.flushBait();
        pbase.rigidBody.velocity = Vector2.zero;
        killRegen = 0;
        dungeonLevel = 0;
        SceneManager.LoadScene("DeathScreen");
        Destroy(gameObject, 0.1f);
        Destroy(Camera.main.gameObject, 0.1f);
    }

    public override void OnKill(EntityStats victim)
    {
        if (victim.killRegenMult > 0.0f)
        {
            weaponInv?.OnKill(victim);
            pbase.OnKill(victim);
        }
        increaseKillRegen(0.1f * victim.killRegenMult);
    }

}
