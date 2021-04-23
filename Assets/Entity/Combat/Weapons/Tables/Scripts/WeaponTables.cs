using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTables", menuName = "ScriptableObjects/TableSet/WeaponTables", order = 1)]
public class WeaponTables : ScriptableObject
{
    // Main Tables
    public IntWeaponClassTable ammoPerKill;
    public FloatWeaponClassTable coolDown;
    public IntWeaponClassTable damage;
    public IntWeaponClassTable maxAmmo;
    public FloatWeaponClassTable projectileLifeTime;
    public FloatWeaponClassTable projectileSpeed;
    public FloatWeaponClassTable staminaCost;
    public TagWeaponClassTable tagWeapon;

    public IntWeaponClassTable multiShotExtraShots;
    public FloatWeaponClassTable multiShotAngle;

    public IntWeaponClassTable burstFireExtraShots;
    public FloatWeaponClassTable burstFireFrequency;

    public FloatWeaponClassTable swingAngle;
    public FloatWeaponClassTable swingHold;

    public IntWeaponClassTable pierce;
    public FloatWeaponClassTable revertTime;
    public FloatWeaponClassTable whipLinkSpeed;
    public WhipLinkOBJ whipLinkObj;

    public SoundTable sounds;

    public DescrWeaponClassTable about;

}
