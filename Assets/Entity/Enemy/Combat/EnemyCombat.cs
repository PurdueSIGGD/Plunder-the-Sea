using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    //[HideInInspector]
    public EnemyBase myBase;
    public float attackRange = 0.5f;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
    }

    // Called right before the enemy dies.
    public virtual void OnDeath()
    {
        // Do nothing by default
    }

    // Called when a projectile hits the enemy. Return true if this should stop the collision.
    public virtual bool OnProjectileHit(Projectile hit)
    {
        return false;
    }

    // Called right after the enemy teleports.
    public virtual void AfterTeleport()
    {
        // Do nothing by default
    }

    // Called right before the enemy teleports.
    public virtual void BeforeTeleport()
    {
        // Do nothing by default
    }
}
