using UnityEngine;

[CreateAssetMenu(fileName = "StaminaProjSys", menuName = "ScriptableObjects/ProjectileSystems/Stamina", order = 1)]
public class StaminaProjSys : ProjectileSystem {
    [SerializeField]
    private float staminaCost;

    private PlayerStats stats;

    public override bool CanShoot(GameObject player) {
        return stats.stamina >= staminaCost;
    }

    public override void OnFire(Projectile projectile)
    {
        stats.UseStamina(staminaCost);
    }

    public override void OnEquip(WeaponInventory inv)
    {
        stats = inv.GetComponent<PlayerBase>().stats;
    }
}