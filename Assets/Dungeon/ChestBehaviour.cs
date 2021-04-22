using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public WeaponTables weaponTables;
    [SerializeField]
    private WeaponFactory.CLASS weaponClass;
    private bool used = false;
    private DescriptionInfo descrInfo;

    void Start()
    {
        descrInfo = GetComponent<DescriptionInfo>();
        SetDescription();
    }

    public void SetDescription()
    {
        if (descrInfo == null)
        {
            return;
        }
        string descr = weaponTables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE ? "Melee: " : "Range: ";
        descr += weaponTables.about.getName(weaponClass) + "\nBase Damage: ";
        descr += weaponTables.damage.get(weaponClass) + "\n\nDescription:\n";
        descr += weaponTables.about.getDescr(weaponClass);
        descrInfo.description = descr;
    }

    public void SetWeaponClass(WeaponFactory.CLASS weaponClass) {
        this.weaponClass = weaponClass;
        SetDescription();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        WeaponInventory weaponInv = collider.GetComponent<WeaponInventory>();
        PlayerStats pStats = collider.GetComponent<PlayerStats>();
        if (weaponInv == null) {
            return;
        }

        int currAmmo = pStats.ammo;
        weaponClass = weaponInv.SetWeapon(weaponClass);
        SetDescription();
        
        if (!used)
        {
            transform.localScale *= 0.5f;
            PlayerPrefs.SetInt("Chests", PlayerPrefs.GetInt("Chests") + 1);
            used = true;
            return;
        }
        pStats.safeSetAmmo(currAmmo);
        
        //this.enabled = false;
    }

}
