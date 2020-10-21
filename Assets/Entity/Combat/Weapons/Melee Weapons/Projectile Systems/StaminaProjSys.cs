using UnityEngine;

[CreateAssetMenu(fileName = "StaminaProjSys", menuName = "ScriptableObjects/ProjectileSystems/Stamina", order = 1)]
public class StaminaProjSys : ProjectileSystem {

    private PlayerStats stats;

    public override bool CanShoot(GameObject player) {
        return stats.stamina >= weapon.staminaCost;
    }

    public override void OnFire(Projectile projectile)
    {
        stats.UseStamina(weapon.staminaCost);
    }

    public override void OnEquip(WeaponInventory inv)
    {
        stats = inv.GetComponent<PlayerBase>().stats;
    }
}