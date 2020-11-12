using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private ScriptableWeapon meleeWeapon;
    [SerializeField]
    private ScriptableWeapon rangeWeapon;
    private UI_Camera cam;
    private SpriteRenderer spriteRen;

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        spriteRen = new GameObject().AddComponent<SpriteRenderer>();
        spriteRen.transform.localScale = this.transform.localScale * 1.5f;
        spriteRen.transform.position = this.transform.position;
        spriteRen.transform.SetParent(this.transform);
        if (meleeWeapon)
        {
            meleeWeapon.OnEquip(this);
        }
        if (rangeWeapon)
        {
            rangeWeapon.OnEquip(this);
            spriteRen.sprite = rangeWeapon.gunSprite;
        }
    }

    private void Update()
    {
        meleeWeapon.Update();
        rangeWeapon.Update();
    }

    private void LateUpdate()
    {
        spriteRen.transform.rotation = Quaternion.FromToRotation(new Vector3(1,1,0), (Vector3)cam.GetMousePosition() - spriteRen.transform.position);
    }

    public ScriptableWeapon GetMelee()
    {
        return meleeWeapon;
    }

    public ScriptableWeapon GetRanged()
    {
        return rangeWeapon;
    }

    public void SetMelee(ScriptableWeapon wep)
    {
        meleeWeapon.OnUnequip(this);
        wep.OnEquip(this);
        // Make sure to set weapon after the equip methods run
        meleeWeapon = wep;
    }

    public void SetRanged(ScriptableWeapon wep)
    {
        rangeWeapon.OnUnequip(this);
        wep.OnEquip(this);
        // Make sure to set weapon after the equip methods run
        rangeWeapon = wep;
        spriteRen.sprite = rangeWeapon.gunSprite;
    }

}
