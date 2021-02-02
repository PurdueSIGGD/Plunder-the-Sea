using UnityEngine;

[CreateAssetMenu(fileName = "CooldownProjSys", menuName = "ScriptableObjects/ProjectileSystems/Cooldown", order = 1)]
public class CooldownProjSys : ProjectileSystem {
    public float cooldownLength = 0.3f;
    private float cooldownEnd = 0.0f;
    protected override void OnEquip(WeaponInventory inv)
    {
        this.cooldownLength = this.tables.coolDown.get(this.weaponClass).Value;
        cooldownEnd = Time.time;
    }

    public override bool CanShoot(GameObject player)
    {
        return Time.time >= cooldownEnd;
    }

    public override void OnFire(Projectile projectile)
    {
        cooldownEnd = Time.time + cooldownLength;
    }

    // only weapons with the "cool down" stat can use this system
    static CooldownProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.coolDown.get(c).HasValue) ? new CooldownProjSys() : null
        );
}