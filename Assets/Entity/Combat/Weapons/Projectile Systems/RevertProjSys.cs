using UnityEngine;

[CreateAssetMenu(fileName = "RevertProjSys", menuName = "ScriptableObjects/ProjectileSystems/Revert", order = 1)]
public class RevertProjSys : ProjectileSystem
{
    public float duration;
    public EntityAttribute[] entityAttributes;
    private EntityStats entityStats;
    private WeaponFactory.CLASS preClassWeapon;
    private float equipTimeInstant = 0f;

    protected override void OnEquip(WeaponInventory inv) 
    {
        this.duration = this.tables.revertTime.get(this.weaponClass).Value;

        equipTimeInstant = Time.time;
        this.entityStats = inv.GetComponent<EntityStats>();

        foreach (var entAttr in entityAttributes)
        {
            this.entityStats.AddAttribute(entAttr, entityStats);
        }
        preClassWeapon = inv.getMeleeWeaponClass();
    }


    public override void Run(Projectile projectile) 
    {
        if (Time.time - equipTimeInstant >= duration) {
            entityStats.GetComponent<WeaponInventory>().SetWeapon(preClassWeapon);
        }
    }

    
    // only weapons with the "revert time" stat can use this system
    static RevertProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.revertTime.get(c).HasValue) ? new RevertProjSys() : null
        );
}