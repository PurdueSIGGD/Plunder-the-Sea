using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private ScriptableWeapon meleeWeapon;
    [SerializeField]
    private ScriptableWeapon rangeWeapon;

    private void Start()
    {
        if (meleeWeapon)
        {
            meleeWeapon.OnEquip(this);
        }
        if (rangeWeapon)
        {
            rangeWeapon.OnEquip(this);
        }
    }

    private void Update()
    {
        meleeWeapon.Update();
        rangeWeapon.Update();
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
    }

}
