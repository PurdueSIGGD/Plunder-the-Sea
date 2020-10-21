using UnityEngine;

[CreateAssetMenu(fileName = "ParentProjSys", menuName = "ScriptableObjects/ProjectileSystems/Parent", order = 1)]
public class ParentProjSys : ProjectileSystem {

    private Transform originTrans;

    public override void OnFire(Projectile projectile)
    {
        projectile.transform.SetParent(originTrans);
    }

    public override void OnEquip(WeaponInventory inv)
    {
        originTrans = inv.transform;
    }
}