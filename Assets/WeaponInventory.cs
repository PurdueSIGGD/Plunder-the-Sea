using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public ScriptableWeapon meleeWeapon;
    public ScriptableWeapon rangeWeapon;

    public ScriptableWeapon testWeaponSlot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var rangeWeaponStart = rangeWeapon;
            rangeWeapon = testWeaponSlot;
            testWeaponSlot = rangeWeaponStart;
        }

    }
}
