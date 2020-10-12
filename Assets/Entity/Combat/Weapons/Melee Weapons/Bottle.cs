using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bottle", menuName = "ScriptableObjects/Bottle", order = 1)]
public class Bottle : ScriptableWeapon
{
    public float duration;
    public float equipTimeInstant = 0f;

    public EntityAttribute[] entityAttributes;
    public EntityStats entityStats;
    public ScriptableWeapon preWeapon;
    public bool equipped;

    public override void Update() 
    {
        if (equipped) {
            Debug.Log(preWeapon.name);
        }
        if (equipped && Time.time - equipTimeInstant >= duration) {
            entityStats.GetComponent<WeaponInventory>().SetMelee(preWeapon);
        }
    }

    public override void OnEquip(WeaponInventory inv) 
    {
        equipped = true;
        Debug.Log("Equip");
        equipTimeInstant = Time.time;
        this.entityStats = inv.GetComponent<EntityStats>();

        foreach (var entAttr in entityAttributes)
        {
            this.entityStats.AddAttribute(entAttr);
        }
        preWeapon = inv.GetMelee();
    }
    public override void OnUnequip(WeaponInventory inv)
    {
        Debug.Log("WTF");
        equipped = false;
    }
}
