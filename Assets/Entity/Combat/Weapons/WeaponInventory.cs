using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private Weapon meleeWeapon;
    [SerializeField]
    private Weapon rangeWeapon;
    private UI_Camera cam;
    private SpriteRenderer spriteRen;
    public GameObject meleeWeaponPrefab {get; private set;}
    public GameObject rangeWeaponPrefab {get; private set;}
    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        spriteRen = new GameObject().AddComponent<SpriteRenderer>();
        spriteRen.transform.localScale = this.transform.localScale * 1.5f;
        spriteRen.transform.position = this.transform.position;
        spriteRen.transform.SetParent(this.transform);
        if (meleeWeapon != null)
        {
            meleeWeapon.OnEquip(this);
        }
        if (rangeWeapon != null)
        {
            rangeWeapon.OnEquip(this);
            // spriteRen.sprite = rangeWeapon.gunSprite;
        }
    }

    private void Update()
    {
        meleeWeapon?.Update();
        rangeWeapon?.Update();
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
        return rangeWeapon;
    }
    public void SetWeaponPrefab(GameObject prefab) {
        var weaponBehaviour = prefab.GetComponent<WeaponBehaviour>();
        var weapon = WeaponFactory.MakeWeapon(weaponBehaviour.weaponClass);

        if (weapon.isMelee) {
            this.SetMelee(weapon);
            this.meleeWeaponPrefab = prefab;
        } 
        else {
            this.SetRanged(weapon);
            this.rangeWeaponPrefab = prefab;
            rangeWeaponPrefab.transform.SetParent(transform);
        }
    }

    public WeaponBaseStats GetWeaponStats(bool melee) {
        if (melee) {
            return meleeWeaponPrefab?.GetComponent<WeaponBehaviour>().stats;
        } else {
            return rangeWeaponPrefab?.GetComponent<WeaponBehaviour>().stats;
        }
    }
    
    private void SetMelee(Weapon wep)
    {
        meleeWeapon.OnUnequip(this);
        wep.OnEquip(this);
        // Make sure to set weapon after the equip methods run
        meleeWeapon = wep;
    }

    private void SetRanged(Weapon wep)
    {
        rangeWeapon?.OnUnequip(this);
        wep.OnEquip(this);
        // Make sure to set weapon after the equip methods run
        rangeWeapon = wep;
        // spriteRen.sprite = rangeWeapon.gunSprite;
    }

}
