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
        if (exploded)
        {
            // Disappear after exploding
            sprite.enabled = false;
            anim.enabled = false;
        }
        

        if (exploded && OnTarget(explodeTarget))
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
        }

        // Handle exploding animation
        if (current == activating)
        {
            anim.SetInteger("State", 2);
        }

        // Explodes the frame the boom fish finishes activating
        if (!exploded && current == cooldown)
        {
            Explode();
            exploded = true;
            explodeTarget = SetTarget(lingerTime);
            GetComponentInChildren<Canvas>().enabled = false;
        }
        prevState = current;
    }

    // Create an explosion projectile, then die at the end of the frame
    void Explode()
    {
        Shoot(explosion);
    }
}
