using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private ScriptableWeapon meleeWeapon;
    [SerializeField]
    private ScriptableWeapon rangeWeapon;

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
        meleeWeapon = wep;
        meleeWeapon.OnEquip(this);
    }

    public void SetRanged(ScriptableWeapon wep)
    {
        rangeWeapon.OnUnequip(this);
        rangeWeapon = wep;
        rangeWeapon.OnEquip(this);
    }

}
