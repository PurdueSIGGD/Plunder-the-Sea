﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomFish : StateCombat
{
    // Explosion projectile
    public GameObject explosion;

    private bool exploded = false;
    private float explodeTarget = 0.0f;
    public float lingerTime = 0.1f;

    // Const values to make coding easier
    const int cooldown = (int)ApproachMovement.ApproachState.cooldown;
    const int activating = (int)ApproachMovement.ApproachState.activating;

    // Update is called once per frame
    void Update()
    {
        if (exploded && OnTarget(explodeTarget))
        {
            Destroy(gameObject);
        }

        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        // Explodes the frame the boom fish finishes activating
        if (current == cooldown && prevState == activating)
        {
            Explode();
            exploded = true;
            explodeTarget = SetTarget(lingerTime);
        }
        prevState = current;
    }

    // Create an explosion projectile, then die at the end of the frame
    void Explode()
    {
        Projectile boom = Projectile.Shoot(explosion, transform.position, myBase.player.transform.position, 0.0f);
        boom.source = gameObject;
    }
}
