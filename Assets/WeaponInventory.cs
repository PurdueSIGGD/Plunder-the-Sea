using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private ScriptableWeapon meleeWeapon;
    [SerializeField]
    private ScriptableWeapon rangeWeapon;

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
        meleeWeapon = wep;
    }

    public void SetRanged(ScriptableWeapon wep)
    {
        rangeWeapon = wep;
    }

}
