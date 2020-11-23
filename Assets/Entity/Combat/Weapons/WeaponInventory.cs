using System.Collections;
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
    private Weapon meleeWeapon;
    private Weapon rangedWeapon;
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
    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();

        spriteRen = new GameObject().AddComponent<SpriteRenderer>();
        spriteRen.transform.localScale = this.transform.localScale * 1.5f;
        spriteRen.transform.position = this.transform.position;
        spriteRen.transform.SetParent(this.transform);

        SetWeapon(this.meleeWeaponClass);
        SetWeapon(this.rangedWeaponClass);

        meleeWeapon?.OnEquip(this);
        rangedWeapon?.OnEquip(this);
   }

    public void OnKill(EntityStats victim) {
        this.rangedWeapon?.OnKill(victim);
        this.meleeWeapon?.OnKill(victim);
    }
    private void Update()
    {
        meleeWeapon?.Update();
        rangedWeapon?.Update();
    }

    private void LateUpdate()
    {
        spriteRen.transform.rotation = Quaternion.FromToRotation(new Vector3(1,1,0), (Vector3)cam.GetMousePosition() - spriteRen.transform.position);
    }

    public Weapon GetMelee()
    {
        return meleeWeapon;
    }

    public Weapon GetRanged()
    {
        return rangedWeapon;
    }
   
    private void SetMelee(Weapon wep)
    {
        meleeWeapon?.OnUnequip(this);
        wep.OnEquip(this);
        // Make sure to set weapon after the equip methods run
        meleeWeapon = wep;
    }

    private void SetRanged(Weapon wep)
    {
        rangedWeapon?.OnUnequip(this);
        wep.OnEquip(this);
        // Make sure to set weapon after the equip methods run
        rangedWeapon = wep;
        spriteRen.sprite = rangedWeaponSpritesTable.get(this.rangedWeaponClass);
    }
    public void SetWeapon(WeaponFactory.CLASS weaponClass) {
        var weapon = WeaponFactory.MakeWeapon(weaponClass);

        if (weapon.isMelee){ 
            this.meleeWeaponClass = weaponClass;
            SetMelee(weapon);
        } else {
            this.rangedWeaponClass = weaponClass;
            SetRanged(weapon);
        }
    }
    public ProjectileStats constructProjectileStats(Weapon weapon, WeaponFactory.CLASS weaponClass) {
        var pStats = new ProjectileStats(){
            prefab = bulletTemplate, damage = damageTable.get(weaponClass), lifeTime = 5f
            };
        if (weapon.isMelee) {
            pStats.prefab = projectilePrefabTable.get(weaponClass);
        }
        pStats.lifeTime = projectileLifeTimesTable.get(weaponClass);

        return pStats;
    }
    public bool ShootAt(Vector2 position, bool isMelee)
    {
        var weaponClass = isMelee ? this.meleeWeaponClass : this.rangedWeaponClass;
        var weapon = isMelee ? this.meleeWeapon : this.rangedWeapon;
        var stats = constructProjectileStats(weapon, weaponClass);
        if (weapon?.CanShoot(this.gameObject) == true)
        {
            Projectile hitbox = Projectile.Shoot(stats.prefab, this.gameObject, position);
            hitbox.weapon = weapon;
            hitbox.damage = stats.damage;
            hitbox.lifeTime = stats.lifeTime;
        
            if (!isMelee) {
                var direction = (position - (Vector2)transform.position).normalized;
                hitbox.GetComponent<Rigidbody2D>().velocity = 
                    direction * weapon.projectileSpeed;
            } 
            weapon.OnFire(hitbox);
            return true;
        }
        return false;
    }

} 