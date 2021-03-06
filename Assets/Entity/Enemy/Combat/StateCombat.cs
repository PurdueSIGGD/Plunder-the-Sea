﻿using System.Collections;
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

    public GameObject eliteSparkle;

    // Elite status effects
    [HideInInspector]
    private static EntityAttribute[] eliteAttributes = {
        new EntityAttribute(ENT_ATTR.ARMOR_STATIC,5f),
        new EntityAttribute(ENT_ATTR.MOVESPEED,1.5f,float.PositiveInfinity,false,false),
        new EntityAttribute(ENT_ATTR.REGEN,0.05f)
    };

    // The distance used by isPlayerUp to determine if an enemy is truly "below" the player, used to make animations bias towards front-facing ones.
    public static float belowThreshold = 0.75f;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        prevState = GetState();
        if (myBase && myBase.myStats && myBase.myStats.elite)
        {
            MakeElite(myBase.myStats.eliteRank);
        }
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

    public void playSound(int i)
    {
        dupeAudio DA = gameObject.AddComponent<dupeAudio>();
        DA.doDupe(GetComponents<AudioSource>()[i]);
    }

    // Makes the enemy into an elite enemy, a rare and powerful version of an enemy. Usually overridden to add some unique additional effects.
    public virtual void MakeElite(int numEffects)
    {
        // Increased health, damage, and kill regen multiplier gain
        myBase.myStats.elite = true;
        myBase.myStats.maxHP *= 3;
        myBase.myStats.currentHP *= 3;
        myBase.myStats.killRegenMult *= 3;
        myBase.myStats.damage *= 2;
        
        // Increased size
        transform.localScale = new Vector3(3.0f, 3.0f, 0);

        // Gets 0-3 random attributes based on numEffects (default is armor, speed, and regen)
        int rand = Random.Range(0, eliteAttributes.Length);
        Color tint = new Color();
        switch (numEffects)
        {
            case 1:
                // Adds 1 random effect
                myBase.myStats.AddAttribute(eliteAttributes[rand], myBase.myStats);
                switch (rand)
                {
                    case 0:
                        tint = new Color(1f, 1f, 0);  // Armor is yellow
                        break;
                    case 1:
                        tint = new Color(0, 1f, 1f);    // Speed is cyan
                        break;
                    case 2:
                        tint = new Color(1f, 0, 1f);  // Regen is magenta
                        break;
                }
                break;
            case 2:
                // Adds all but 1 random effect
                for (int i = 0; i < eliteAttributes.Length; i++)
                {
                    if (i == rand) continue;
                    myBase.myStats.AddAttribute(eliteAttributes[i], myBase.myStats);
                }
                switch (rand)
                {
                    case 0:
                        tint = new Color(0, 0, 1f);  // Speed/Regen is blue
                        break;
                    case 1:
                        tint = new Color(1f, 0, 0);    // Armor/Regen is red
                        break;
                    case 2:
                        tint = new Color(0, 1f, 0);  // Armor/Speed is green
                        break;
                }
                break;
            case 3:
                // Adds all effects
                foreach (EntityAttribute attr in eliteAttributes)
                {
                    myBase.myStats.AddAttribute(attr, myBase.myStats);
                }
                tint = new Color(1f, 1f, 1f);  // All three is white (will be rainbow anyway)
                break;
            default:
                tint = new Color(1f, 1f, 1f);
                break;
        }
        // Creates a SpawnSparkle component with the tint color
        SpawnSparkle sparkle = gameObject.AddComponent<SpawnSparkle>();
        sparkle.size = transform.localScale.x * 0.2f;
        sparkle.sparkle = eliteSparkle;
        sparkle.rate = 0.15f;
        sparkle.sparkleColor = tint;
        if (numEffects == 3)
        {
            sparkle.rainbow = true;
        }

        myBase.myStats.displayName = "Elite " + myBase.myStats.displayName;
    }
}
