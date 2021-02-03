using UnityEngine;

[CreateAssetMenu(fileName = "ParentProjSys", menuName = "ScriptableObjects/ProjectileSystems/Parent", order = 1)]
public class ParentProjSys : ProjectileSystem {

    public override void OnFire(Projectile projectile)
    {
        projectile.transform.SetParent(projectile.source.transform);
    }

    // only melee weapons can use this system
    static ParentProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => 
                (t.tagWeapon.get(c) == WeaponFactory.TAG.MELEE) ? 
                new ParentProjSys() : null
        );
}