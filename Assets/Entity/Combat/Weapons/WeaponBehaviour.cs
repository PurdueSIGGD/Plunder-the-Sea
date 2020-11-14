using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
public class WeaponBehaviour : MonoBehaviour {
    [SerializeField]
    private WeaponBaseStats stats;

    [SerializeField]
    private WeaponFactory.CLASS weaponClass;

    private Weapon weapon;
    
    private void Start() {

        this.weapon = WeaponFactory.MakeWeapon(this.weaponClass);
    }
}