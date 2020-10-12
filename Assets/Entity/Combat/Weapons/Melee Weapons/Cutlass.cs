using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutlass", menuName = "ScriptableObjects/Cutlass", order = 1)]
public class Cutlass : ScriptableWeapon
{

    public float sweepAngle = 180.0f;

    private List<Projectile> projList = new List<Projectile>();

    public override void Update()
    {
        foreach (Projectile proj in projList) {
            proj.transform.Rotate(0, 0, Time.deltaTime * sweepAngle / proj.lifeTime);
        }
    }
    
    
    public override void OnFire(Projectile projectile)
    {
        projList.Add(projectile);
        projectile.transform.Rotate(0, 0, -sweepAngle / 2);
    }

    public override void OnEnd(Projectile projectile)
    {
        projList.Remove(projectile);
    }

}
