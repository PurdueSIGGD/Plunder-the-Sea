using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCombat : EnemyCombat
{
    // A base combat class to be combined with a StateMovement class (to take advantae of movement states) with utility functions
    [HideInInspector]
    public int prevState;
    [HideInInspector]
    public StateMovement myStateMovement;

    // Sprite References
    public SpriteRenderer sprite;
    public Animator anim;

    // The distance used by isPlayerUp to determine if an enemy is truly "below" the player, used to make animations bias towards front-facing ones.
    public static float belowThreshold = 0.75f;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        prevState = GetState();
        CombatStart();
    }

    // Called by Start() to allow enemies to perform their own actions
    public virtual void CombatStart()
    {
        // Do nothing by default
    }

    // Get the current state by calling the state of the movement
    public int GetState()
    {
        return myStateMovement.GetState();
    }

    // Set the current state by setting the state of the movement
    public void SetState(int newState)
    {
        myStateMovement.SetState(newState);
    }

    // Return the target time into a variable (similar to StateMovement, but for time-based cooldowns in general)
    public static float SetTarget(float elapsedTime)
    {
        return Time.time + elapsedTime;
    }

    // Return whether the timer has passed the goal time (ideal when used with a return value from SetTarget)
    public static bool OnTarget(float goalTime)
    {
        return (Time.time > goalTime);
    }

    // Fire a projectile towards a direction, and ensure it functions like an enemy projectile
    public Projectile Shoot(GameObject projectile, Vector2 startPos, Vector2 target, bool inheritDamage = false)
    {
        Projectile shot = Projectile.Shoot(projectile, startPos, target);
        shot.SetSource(myBase.gameObject);

        Vector2 direction = (target - startPos).normalized;
        shot.GetComponent<Rigidbody2D>().velocity = direction * shot.speed;
        shot.tables = null;
        shot.weaponSystem = null;
        if (inheritDamage)
        {
            shot.damage = myBase.myStats.damage;
        }
        return shot;
    }

    // Reduction of previous method to assumedly shoot from the enemy to the player
    public Projectile Shoot(GameObject projectile, bool inheritDamage = false)
    {
        return Shoot(projectile, transform.position, myBase.player.transform.position, inheritDamage);
    }

    // Returns true if the enemy should be facing left to face the player. Otherwise returns false.
    public bool isPlayerLeft()
    {
        if (myBase.player)
        {
            return (myBase.player.transform.position.x < transform.position.x);
        }
        return false;
    }

    // Returns true if the enemy should be facing up to face the player (i.e the player is behind them). Otherwise returns false.
    public bool isPlayerUp()
    {
        if (myBase.player)
        {
            return ((myBase.player.transform.position.y - belowThreshold) > transform.position.y);
        }
        return false;
    }
}
