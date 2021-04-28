using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : StateCombat
{
    // Stats about the most recent and overall projectile damage (including melee)
    public float prevDamage = 0f;
    public int prevType = 1;
    public float totalDamage = 0f;

    // Variable for how long after the last damage instance
    public float totalInterval = 3f;

    // The target does nothing by default, but records the most recent projectiles hitting it
    public override bool OnProjectileHit(Projectile hit)
    {
        prevType = hit.ProjectileType();

        return false;
    }
}
