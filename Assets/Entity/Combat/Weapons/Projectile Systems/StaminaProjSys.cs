using UnityEngine;

[CreateAssetMenu(fileName = "StaminaProjSys", menuName = "ScriptableObjects/ProjectileSystems/Stamina", order = 1)]
public class StaminaProjSys : ProjectileSystem {
    public float staminaCost;
    private PlayerStats stats;
    protected override void OnEquip(WeaponInventory inv)
    {
        this.staminaCost = this.tables.staminaCost.get(this.weaponClass).Value;
        stats = inv.GetComponent<PlayerBase>().stats;
    }

    public override bool CanShoot(GameObject player) {
        //The return was giving Null reference errors, this is a quick and dirty fix, but should stop them from occuring
        if (!stats)
        {
            stats = player.GetComponent<PlayerBase>().stats;
        }
        return stats.stamina >= staminaCost;
    }

    public override void OnFire(Projectile projectile)
    {
        stats.UseStamina(staminaCost);
    }

    // only weapons with the "staminaCost" stat can use this system
    static StaminaProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.staminaCost.get(c).HasValue) ? new StaminaProjSys() : null
        );
}