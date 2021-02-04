using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "MultishotProjSys", menuName = "ScriptableObjects/ProjectileSystems/Multishot", order = 1)]
public class MultishotProjSys : ProjectileSystem {

    public int extraShots = 1;
    public float angle = 10.0f;

    protected override void OnEquip(WeaponInventory inv) {
        this.extraShots = this.tables.multiShotExtraShots.get(this.weaponClass).Value;
        this.angle = this.tables.multiShotAngle.get(this.weaponClass).Value;
    }

    public override void OnFire(Projectile projectile)
    {
        Rigidbody2D origRigid = projectile.GetComponent<Rigidbody2D>();
        Vector3 origVel = origRigid.velocity;
        for (int i = 0; i < extraShots; i++)
        {
            //Alternate angle directions
            float currentAngle = angle * (i / 2 + 1) * (i % 2 == 0 ? 1 : -1);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Projectile newShot = Instantiate(projectile);
            Rigidbody2D newRigid = newShot.GetComponent<Rigidbody2D>();
            newRigid.velocity = rotation * origVel;
        }
    }

    // only weapons with the multishot stats can use this system
    static MultishotProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => 
                (t.multiShotExtraShots.get(c).HasValue && t.multiShotAngle.get(c).HasValue) 
                ? new MultishotProjSys() : null
        );
}