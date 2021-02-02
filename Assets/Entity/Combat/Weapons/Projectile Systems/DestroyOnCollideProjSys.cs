using UnityEngine;

[CreateAssetMenu(fileName = "DestroyOnCollideProjSys", menuName = "ScriptableObjects/ProjectileSystems/DestroyOnCollide", order = 1)]
public class DestroyOnCollideProjSys : ProjectileSystem {

    public bool destroyOnCollide;

    public DestroyOnCollideProjSys(bool destroyOnCollide) {
        this.destroyOnCollide = destroyOnCollide;
    }

    public override void OnFire(Projectile projectile)
    {
        projectile.destroyOnCollide = destroyOnCollide;
    }

    // only melee weapons can use this system
    static DestroyOnCollideProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => 
                (t.tagWeapon.get(c) == WeaponFactory.TAG.MELEE) ? 
                new DestroyOnCollideProjSys(false) : null
        );

}