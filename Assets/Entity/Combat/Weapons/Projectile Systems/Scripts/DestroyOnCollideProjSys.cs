using UnityEngine;

[CreateAssetMenu(fileName = "DestroyOnCollideProjSys", menuName = "ScriptableObjects/ProjectileSystems/DestroyOnCollide", order = 1)]
public class DestroyOnCollideProjSys : ProjectileSystem {

    public bool destroyOnCollide;
    public override void OnFire(Projectile projectile)
    {
        projectile.destroyOnCollide = destroyOnCollide;
    }
}