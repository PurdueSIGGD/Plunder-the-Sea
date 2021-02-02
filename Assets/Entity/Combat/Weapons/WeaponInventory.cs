﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private WeaponFactory.CLASS meleeWeaponClass;
    public WeaponFactory.CLASS getMeleeWeaponClass(){ 
        return this.meleeWeaponClass;
    }
    [SerializeField]
    private WeaponFactory.CLASS rangedWeaponClass;
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
    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();

        spriteRen = new GameObject().AddComponent<SpriteRenderer>();
        spriteRen.transform.localScale = this.transform.localScale * 1.5f;
        spriteRen.transform.position = this.transform.position;
        spriteRen.transform.SetParent(this.transform);

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
        spriteRen.sprite = rangedWeaponSpritesTable.get(this.rangedWeaponClass);
    }

    public void SetWeapon(WeaponFactory.CLASS weaponClass) {
        var weapon = new WeaponSystem(weaponClass, this.tables);

        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE){ 
            this.meleeWeaponClass = weaponClass;
            SetMelee(weapon);
        } else {
            this.rangedWeaponClass = weaponClass;
            SetRanged(weapon);
        }
    }
    public ProjectileStats constructProjectileStats(WeaponFactory.CLASS weaponClass) {
        var pStats = new ProjectileStats(){
            prefab = bulletTemplate, damage = damageTable.get(weaponClass).Value, lifeTime = 5f
            };
        if (tables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE) {
            pStats.prefab = projectilePrefabTable.get(weaponClass);
        }
        pStats.lifeTime = projectileLifeTimesTable.get(weaponClass).Value;

        return pStats;
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
        
            if (!isMelee) {
                var direction = (position - (Vector2)transform.position).normalized;
                hitbox.GetComponent<Rigidbody2D>().velocity = 
                    direction * tables.projectileSpeed.get(weaponClass).GetValueOrDefault(0f);
            } 
            weaponSystem.OnFire(hitbox);
            return true;
        }
        return false;
    }

} 