﻿using System.Collections;
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

    //death stats variables
    public int wepsGot = 0;

    [SerializeField]
    private Transform meleeParent;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null)
        {
            audioSrc = gameObject.AddComponent<AudioSource>();
        }

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

    public Sprite GetGunSprite() {
        return this.rangedWeaponSpritesTable.get(this.rangedWeaponClass);
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
    }

    public WeaponFactory.CLASS SetWeapon(WeaponFactory.CLASS weaponClass) {
        var weapon = new WeaponSystem(weaponClass, this.tables);
        WeaponFactory.CLASS old;
        if (equipSound)
        {
            audioSrc.clip = equipSound;
            audioSrc.Play();
        }
        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE){
            old = this.meleeWeaponClass;
            this.meleeWeaponClass = weaponClass;
            SetMelee(weapon);
        } else {
            old = this.rangedWeaponClass;
            this.rangedWeaponClass = weaponClass;
            SetRanged(weapon);
        }
        wepsGot++;
        return old;
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
            //Debug.Log("Scale: " + pStats.prefab.transform.localScale.ToString());
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
    public bool ShootAt(Vector2 position, bool isMelee, bool checkCost = true)
    {
        var weaponClass = isMelee ? this.meleeWeaponClass : this.rangedWeaponClass;
        var weaponSystem = isMelee ? this.meleeSystem : this.rangedSystem;
        var stats = constructProjectileStats(weaponClass);
        if (!checkCost || weaponSystem?.CanShoot(this.gameObject) == true)
        {
            Projectile hitbox = Projectile.Shoot(stats.prefab, this.gameObject, position);
            hitbox.weaponSystem = weaponSystem;
            hitbox.weaponClass = weaponClass;
            hitbox.damage = stats.damage;
            hitbox.tables = this.tables;
            hitbox.lifeTime = stats.lifeTime;
            hitbox.ammoRefill = stats.ammoRefill;
            hitbox.parent = this.meleeParent;
            hitbox.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;

            //Sound stuff
            GameObject g = Instantiate(new GameObject(), transform.position, Quaternion.identity);
            AudioSource newAS = g.AddComponent<AudioSource>();
            newAS.clip = tables.sounds.getSound(weaponClass);
            newAS.volume = tables.sounds.getVolume (weaponClass);
            newAS.pitch = tables.sounds.getPitch(weaponClass);

            newAS.Play();
            Destroy(g, newAS.clip.length + 0.1f);

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

    public float getMaxWait()
    {
        return Mathf.Max(tables.coolDown.get(rangedWeaponClass).Value, tables.projectileLifeTime.get(meleeWeaponClass).Value / 4);
    }

} 