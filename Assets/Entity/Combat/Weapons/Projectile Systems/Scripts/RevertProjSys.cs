using UnityEngine;

[CreateAssetMenu(fileName = "RevertProjSys", menuName = "ScriptableObjects/ProjectileSystems/Revert", order = 1)]
public class RevertProjSys : ProjectileSystem
{
    public float duration;
    public EntityAttribute[] entityAttributes;
    private EntityStats entityStats;
    private ScriptableWeapon preWeapon;
    private float equipTimeInstant = 0f;

    public RevertProjSys(float duration) {
        this.duration = duration;
    }

    public override void Run(Projectile projectile) 
    {
        if (Time.time - equipTimeInstant >= duration) {
            entityStats.GetComponent<WeaponInventory>().SetMelee(preWeapon);
        }
    }

    public override void OnEquip(WeaponInventory inv) 
    {
        equipTimeInstant = Time.time;
        this.entityStats = inv.GetComponent<EntityStats>();

        foreach (var entAttr in entityAttributes)
        {
            this.entityStats.AddAttribute(entAttr, entityStats);
        }
        preWeapon = inv.GetMelee();
    }
}