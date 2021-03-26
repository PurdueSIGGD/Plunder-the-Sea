using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public AudioClip equipSound;
    private AudioSource audioSrc;
    [SerializeField]
    public WeaponFactory.CLASS meleeWeaponClass;
    public WeaponFactory.CLASS getMeleeWeaponClass(){ 
        return this.meleeWeaponClass;
    }
    [SerializeField]
    public WeaponFactory.CLASS rangedWeaponClass;
    public WeaponFactory.CLASS getRangedWeaponClass()
    {
        return this.rangedWeaponClass;
    }
    private WeaponSystem meleeSystem;
    private WeaponSystem rangedSystem;
    private UI_Camera cam;
    private SpriteRenderer spriteRen;

    [SerializeField]
    private IntWeaponClassTable damageTable; 
    [SerializeField]
    private FloatWeaponClassTable projectileLifeTimesTable; 
    [SerializeField]
    private SpritesWeaponClassTable rangedWeaponSpritesTable; 
    [SerializeField]
    private PrefabWeaponClassTable projectilePrefabTable; 

    [SerializeField]
    private GameObject bulletTemplate;
    [SerializeField]
    private WeaponTables tables;

    //stores player weapon modifiers
    public PlayerClasses.WeaponModifiers weaponMods = new PlayerClasses.WeaponModifiers();

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null)
        {
            audioSrc = gameObject.AddComponent<AudioSource>();
        }

        spriteRen = new GameObject().AddComponent<SpriteRenderer>();
        spriteRen.transform.localScale = this.transform.localScale * 0.5f; //gunScale
        spriteRen.transform.position = this.transform.position;
        spriteRen.transform.SetParent(this.transform);

        PlayerClasses PC = GetComponent<PlayerClasses>();
        PC.getMods(weaponMods);
        meleeWeaponClass = PC.melee;
        rangedWeaponClass = PC.ranged;

        SetWeapon(this.meleeWeaponClass);
        SetWeapon(this.rangedWeaponClass);
   }

    public void OnKill(EntityStats victim) {
        this.meleeSystem?.OnKill(victim);
        this.rangedSystem?.OnKill(victim);
    }
    private void Update()
    {
        meleeSystem?.Update();
        rangedSystem?.Update();
    }

    private void LateUpdate()
    {
        spriteRen.transform.rotation = Quaternion.FromToRotation(new Vector3(1,1,0), (Vector3)cam.GetMousePosition() - spriteRen.transform.position);
    }

    private void SetMelee(WeaponSystem wep)
    {
        meleeSystem?.OnUnequip(this);
        wep.OnEquip(this, tables, meleeWeaponClass);
        // Make sure to set weapon after the equip methods run
        meleeSystem = wep;
    }

    private void SetRanged(WeaponSystem wep)
    {
        rangedSystem?.OnUnequip(this);
        wep.OnEquip(this, tables, rangedWeaponClass);
        // Make sure to set weapon after the equip methods run
        rangedSystem = wep;

        //TESTING
        if (spriteRen != null)
        {
            if (rangedWeaponSpritesTable.get(this.rangedWeaponClass))
            {
                spriteRen.sprite = rangedWeaponSpritesTable.get(this.rangedWeaponClass);
            }
            else
            {
                Debug.Log("Sprite not set");
            }
        }
        else
        {
            Debug.Log("Renderer not made");
        }
        //spriteRen.sprite = rangedWeaponSpritesTable.get(this.rangedWeaponClass);
    }

    public void SetWeapon(WeaponFactory.CLASS weaponClass) {
        var weapon = new WeaponSystem(weaponClass, this.tables);
        if (equipSound)
        {
            audioSrc.clip = equipSound;
            audioSrc.Play();
        }
        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE){ 
            this.meleeWeaponClass = weaponClass;
            SetMelee(weapon);
        } else {
            this.rangedWeaponClass = weaponClass;
            SetRanged(weapon);
        }
    }

    public ProjectileStats constructProjectileStats(WeaponFactory.CLASS weaponClass) {

        //sets up assuming a ranged weapon
        var pStats = new ProjectileStats() {
            prefab = bulletTemplate, damage = projectileDamage(weaponClass),
            lifeTime = (weaponMods.projectileLifetimeAddition + projectileLifeTimesTable.get(weaponClass).Value) * weaponMods.projectileLifetimeMultiplier,
            ammoRefill = tables.ammoPerKill.get(weaponClass).Value
            };

        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE) {
            //corrects if melee
            pStats.damage = projectileDamage(weaponClass);
            pStats.lifeTime = projectileLifeTimesTable.get(weaponClass).Value;

            pStats.prefab = projectilePrefabTable.get(weaponClass);
            pStats.prefab.transform.localScale = Vector3.one * (1 + weaponMods.meleeSizeAddition) * weaponMods.meleeSizeMultiplier * 0.5f; //swordScale
        }

        //pStats.lifeTime = projectileLifeTimesTable.get(weaponClass).Value;
        return pStats;
    }

    public int projectileDamage(WeaponFactory.CLASS weaponClass)
    {
        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE)
        {
            return (int)((damageTable.get(weaponClass).Value + weaponMods.meleeDamageAddition) * weaponMods.meleeDamageMultiplier);
        }
        return (int)((damageTable.get(weaponClass).Value + weaponMods.rangedDamageAddition) * weaponMods.rangedDamageMultiplier);
    }

    public Sprite getWeaponImage(bool isMelee)
    {
        if (isMelee)
        {
            GameObject prefab = projectilePrefabTable.get(this.meleeWeaponClass);
            return prefab.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        return rangedWeaponSpritesTable.get(this.rangedWeaponClass);
    }
    public bool ShootAt(Vector2 position, bool isMelee)
    {
        var weaponClass = isMelee ? this.meleeWeaponClass : this.rangedWeaponClass;
        var weaponSystem = isMelee ? this.meleeSystem : this.rangedSystem;
        var stats = constructProjectileStats(weaponClass);
        if (weaponSystem?.CanShoot(this.gameObject) == true)
        {
            Projectile hitbox = Projectile.Shoot(stats.prefab, this.gameObject, position);
            hitbox.weaponSystem = weaponSystem;
            hitbox.weaponClass = weaponClass;
            hitbox.damage = stats.damage;
            hitbox.tables = this.tables;
            hitbox.lifeTime = stats.lifeTime;
            hitbox.ammoRefill = stats.ammoRefill;
        
            if (!isMelee) {
                var direction = (position - (Vector2)transform.position).normalized;
                hitbox.GetComponent<Rigidbody2D>().velocity =
                    direction * (tables.projectileSpeed.get(weaponClass).GetValueOrDefault(0f) + weaponMods.rangedSpeedAddition) * weaponMods.rangedSpeedMultiplier;
            } 
            weaponSystem.OnFire(hitbox);
            return true;
        }
        return false;
    }

} 