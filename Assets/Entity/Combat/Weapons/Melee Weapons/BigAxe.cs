using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BigAxe", menuName = "ScriptableObjects/BigAxe", order = 1)]
public class BigAxe : ScriptableWeapon
{

    public float sweepAngle = 180.0f;
    /* Seconds holding weapon before swing */
    public float holdTime = 0.2f;

    private List<Projectile> projList = new List<Projectile>();

    public override void Update()
    {
        foreach (Projectile proj in projList) {
            if (proj.currentLifeTime > holdTime)
            {
                proj.transform.Rotate(0, 0, Time.deltaTime * sweepAngle / (proj.lifeTime - holdTime));
            }
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
