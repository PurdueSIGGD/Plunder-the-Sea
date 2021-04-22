using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public WeaponTables weaponTables;
    [SerializeField]
    private WeaponFactory.CLASS weaponClass;
    public SpritesWeaponClassTable rangedWeaponSpritesTable;
    public PrefabWeaponClassTable projectilePrefabTable;
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
        string descr = weaponTables.tagWeapon.get(weaponClass) == WeaponFactory.TAG.MELEE ? "Melee" : "Ranged";
        descr += " Damage: ";
        descr += weaponTables.damage.get(weaponClass) + "\n";
        descr += weaponTables.about.getDescr(weaponClass);
        descrInfo.name = weaponTables.about.getName(weaponClass);
        descrInfo.description = descr;

        descrInfo.weaponClass = weaponClass;
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

        /*
        used = true;
        weaponInv.SetWeapon(weaponClass);
        transform.localScale *= 0.5f;

        PlayerPrefs.SetInt("Chests", PlayerPrefs.GetInt("Chests") + 1);
        Destroy(descrInfo);

        this.enabled = false;
        */
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
        
    }

}
