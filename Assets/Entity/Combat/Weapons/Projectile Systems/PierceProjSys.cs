using System;
using UnityEngine;

public class PierceProjSys : ProjectileSystem {

    /* Amount of targets allowed to hit */
    public int pierceAmount;

    public PierceProjSys(int pierceAmount) {
        this.pierceAmount = pierceAmount;
    }

    public override void OnHit(Projectile proj, EntityStats victim)
    {
        proj.destroyOnCollide = (proj.pierceCount >= pierceAmount);
    }

}