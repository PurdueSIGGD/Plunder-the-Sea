using UnityEngine;

[CreateAssetMenu(fileName = "ParentProjSys", menuName = "ScriptableObjects/ProjectileSystems/Parent", order = 1)]
public class ParentProjSys : ProjectileSystem {

    public override void OnFire(Projectile projectile)
    {
        projectile.transform.SetParent(projectile.source.transform);
    }
}