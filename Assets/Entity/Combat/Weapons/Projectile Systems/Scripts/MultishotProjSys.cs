using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "MultishotProjSys", menuName = "ScriptableObjects/ProjectileSystems/Multishot", order = 1)]
public class MultishotProjSys : ProjectileSystem {

    public int extraShots = 1;
    public float angle = 10.0f;

    public MultishotProjSys(int extraShots, float angle) {
        this.extraShots = extraShots;
        this.angle = angle;
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
}