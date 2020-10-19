using UnityEngine;

[CreateAssetMenu(fileName = "CooldownProjSys", menuName = "ScriptableObjects/ProjectileSystems/Cooldown", order = 1)]
public class CooldownProjSys : ProjectileSystem {
    public float cooldownLength = 0.3f;
    private float cooldownEnd = 0.0f;

    public override bool CanShoot(GameObject player)
    {
        return Time.time >= cooldownEnd;
    }

    public override void OnFire(Projectile projectile)
    {
        cooldownEnd = Time.time + cooldownLength;
    }
}