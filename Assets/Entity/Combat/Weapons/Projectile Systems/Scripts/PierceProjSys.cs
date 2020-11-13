using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PierceProjSys", menuName = "ScriptableObjects/ProjectileSystems/Pierce", order = 1)]
public class PierceProjSys : ProjectileSystem {

    /* Amount of targets allowed to hit */
    public int pierceAmount;

    public override void OnHit(Projectile proj, EntityStats victim)
    {
        proj.destroyOnCollide = (proj.pierceCount >= pierceAmount);
    }

}