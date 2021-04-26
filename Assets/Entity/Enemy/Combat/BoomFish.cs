using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomFish : StateCombat
{
    // Explosion projectile
    public GameObject explosion;

    private bool exploded = false;
    private float explodeTarget = 0.0f;
    public float lingerTime = 0f;

    // Const values to make coding easier
    const int cooldown = (int)ApproachMovement.ApproachState.cooldown;
    const int activating = (int)ApproachMovement.ApproachState.activating;
    const int approaching = (int)ApproachMovement.ApproachState.approaching;

    // Update is called once per frame
    void Update()
    {
        if (lingerTime == 0f)
        {
            lingerTime = explosion.GetComponent<Projectile>().lifeTime;
        }
        if (exploded && !myBase.myStats.elite)
        {
            // Disappear after exploding
            sprite.enabled = false;
            anim.enabled = false;
        }

        

        if (exploded && OnTarget(explodeTarget) && !myBase.myStats.elite)
        {
            // Die once explosion finishes
            DestroyImmediate(gameObject);            
        }

        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        // Handle movement animation
        if (current == approaching)
        {
            sprite.flipX = isPlayerLeft();
            anim.SetInteger("State", (isPlayerUp()) ? 1 : 0);
            if (myBase.myStats.elite)
            {
                exploded = false;
                anim.speed = 1f;
            }
        }

        // Handle exploding animation
        if (current == activating)
        {
            anim.SetInteger("State", 2);
            if (myBase.myStats.elite)
            {
                anim.speed = 2f;
            }
        }

        if (current == cooldown && myBase.myStats.elite)
        {
            anim.speed = 0;
        }

        // Explodes the frame the boom fish finishes activating
        if (!exploded && current == cooldown)
        {
            GameObject expObj = Shoot(explosion, true).gameObject;
            if (myBase.myStats.elite)
            {
                // Elite boomfish have twice the explosion size
                expObj.GetComponent<SizeChange>().endSize *= 2f;
            } else
            {
                GetComponentInChildren<Canvas>().enabled = false;
            }
            exploded = true;
            explodeTarget = SetTarget(lingerTime);
        }
        prevState = current;
    }

    public override void MakeElite(int numEffects)
    {
        base.MakeElite(numEffects);
        ApproachMovement am = ((ApproachMovement)myStateMovement);
        am.approachDistance *= 2f;
        am.actChargeUpTime *= 0.5f;
        am.actCooldownTime *= 2f;
    }
}
