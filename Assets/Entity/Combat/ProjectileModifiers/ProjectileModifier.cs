using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModifier : MonoBehaviour
{
    // A script that can be attached to a projectile to give it modified behavior, which will work on all instances of that projectile
    // Special methods used to ensure proper execution order

    public virtual void ProjectileStart()
    {
        // Do nothing by default, this is called when the projectile is spawned
    }

    public virtual void ProjectileUpdate()
    {
        // Do nothing by default, this is called when the projectile exists
    }

    public virtual void ProjectileEnd()
    {
        // Do nothing by default, this is called when the projectile is destroyed
    }
}
