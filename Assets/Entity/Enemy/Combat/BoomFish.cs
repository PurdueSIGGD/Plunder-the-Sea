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

    // Update is called once per frame
    void Update()
    {
        if (lingerTime == 0f)
        {
            lingerTime = explosion.GetComponent<Projectile>().lifeTime;
        }
        

        if (exploded && OnTarget(explodeTarget))
        {
            DestroyImmediate(gameObject);
        }

        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        // Explodes the frame the boom fish finishes activating
        if (!exploded && current == cooldown && prevState == activating)
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
        Shoot(explosion);
    }
}
