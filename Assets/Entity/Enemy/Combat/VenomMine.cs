using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Venom mine is implemented as having approach movement with zero speed for simplicity
public class VenomMine : StateCombat
{
    public Sprite defaultSprite;
    public Sprite detonateSprite;

    // Venom cloud projectile
    public GameObject venomCloud;

    // Variables inherited from BoomFish
    private bool exploded = false;
    private float explodeTarget = 0.0f;
    public float lingerTime = 0.1f;

    // How long the mine lasts on its own (explodes at the end of its duration)
    public float duration = 10.0f;
    private float durationTarget = 0.0f;

    // Const values to make coding easier
    const int cooldown = (int)ApproachMovement.ApproachState.cooldown;
    const int activating = (int)ApproachMovement.ApproachState.activating;

    // Update is called once per frame
    void Update()
    {
        if (lingerTime == 0f)
        {
            lingerTime = venomCloud.GetComponent<Projectile>().lifeTime;
        }

        if (exploded && OnTarget(explodeTarget))
        {
            myBase.myStats.Die();
        }

        if (durationTarget == 0.0f)
        {
            durationTarget = SetTarget(duration);
        }

        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        if (current == activating)
        {
            sprite.sprite = detonateSprite;
            sprite.flipX = sprite.flipY = false;
        } else
        {
            sprite.sprite = defaultSprite;
            sprite.flipX = sprite.flipY = true;
        }

        // Explodes the frame the venom mine finishes activating, or when the duration runs out
        if (!exploded && (OnTarget(durationTarget)) || (!exploded && current == cooldown && prevState == activating))
        {
            Explode();
            exploded = true;
            explodeTarget = SetTarget(lingerTime);
        }
        prevState = current;
    }

    public override void OnDeath()
    {
        if (!exploded)
        {
            Explode();
            exploded = true;
        }
    }

    // Create an venomCloud projectile, then die at the end of the frame
    void Explode()
    {
        Shoot(venomCloud);
    }
}
