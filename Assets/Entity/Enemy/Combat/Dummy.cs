using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : StateCombat
{
    // Stats about the most recent and overall projectile damage (including melee)
    public float prevDamage = 0f;
    public int prevType = 0;
    public float totalDamage = 0f;
    public float totalTime = 0f;

    // Calculated stats
    public float dps;

    // Variable for how long after the last damage instance
    public float totalInterval = 3f;

    // Sprites for hurt/unhurt states
    public Sprite unhurt;
    public Sprite hurt;

    // Description String
    public DescriptionInfo desc;

    private string[] types = { "N/A", "Melee", "Ranged" };

    // Stats used for damage calculation
    private float endTimer = 0f;
    private float prevTime = 0f;
    private float firstDamage = 0;
    private bool isHurt;

    public override void CombatStart()
    {
        UpdateDesc();
    }

    private void Update()
    {
        if (Time.time >= endTimer)
        {
            sprite.sprite = unhurt;
            totalDamage = 0f;
            totalTime = 0f;
            dps = 0f;
            isHurt = false;
            UpdateDesc();
        } else
        {
            sprite.sprite = hurt;
        }
    }

    public void UpdateDesc()
    {
        desc.description =
            "DPS: " + Mathf.Round(dps) + "\n" +
            "Prev. Hit: " + types[prevType] + "\n" +
            "Prev. Damage: " + Mathf.Round(prevDamage) + "\n" +
            "Total Damage: " + Mathf.Round(totalDamage) + "\n" +
            "A kind fish that describes how hard your attacks hit in exchange for free bait.";
        desc.updateInventoryDescription();
    }

    // The target does nothing by default, but records the most recent projectiles hitting it
    // This pre-calculates the damage, meaning it ignores invulnerability
    public override bool OnProjectileHit(Projectile hit)
    {
        prevType = hit.ProjectileType();
        
        float amount = hit.damage;

        // Dynamically calculate damage
        float multiplier;
        float realDmg = amount;
        int level = FindObjectOfType<PlayerStats>().dungeonLevel;
        multiplier = 1 / (1 + .3f * level);
        realDmg *= multiplier;

        prevDamage = realDmg;
        totalDamage += realDmg;
        
        // Update DPS if already hurt
        if (isHurt)
        {
            totalTime += Time.time - prevTime;
            if (totalTime != 0)
            {
                dps = Mathf.Round((totalDamage - firstDamage) / totalTime);
                UpdateDesc();
            }
        } else
        {
            // Start DPS otherwise
            isHurt = true;
            firstDamage = realDmg;
        }

        prevTime = Time.time;
        endTimer = Time.time + totalInterval;

        if (myBase.myStats.invulnerable && hit.ProjectileType() == 1)
        {
            // Refill ammo on hit for overworld dummy
            myBase.player.stats.replenishAmmo(1000);
        }

        return false;
    }

    // Updates text description

}
