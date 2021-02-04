using UnityEngine;

[CreateAssetMenu(fileName = "TrustProjSys", menuName = "ScriptableObjects/ProjectileSystems/Thrust", order = 1)]
public class ThrustProjSys : ProjectileSystem
{
    public override void Run(Projectile projectile)
    {
        Vector3 dir = projectile.transform.rotation * Vector3.right;
        if (projectile.currentLifeTime >= projectile.lifeTime / 2)
        {
            dir *= -1;
        }
        SpriteRenderer sprite = projectile.GetComponentInChildren<SpriteRenderer>();
        float delta = Time.deltaTime / projectile.lifeTime;
        float magnitude = sprite.bounds.size.x;
        projectile.transform.localPosition += dir * delta * magnitude;
    }

    public override void OnFire(Projectile projectile) { }

    // only a select few weapons can use this system
    static ThrustProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => 
                (c == WeaponFactory.CLASS.SPEAR || c == WeaponFactory.CLASS.DAGGER) 
                ? new ThrustProjSys() : null
        );
}