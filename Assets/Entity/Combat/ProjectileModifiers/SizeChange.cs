using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChange : ProjectileModifier
{
    // A projectile with SizeChange can change sizes as they exist, either growing, or shrinking.

    public float startSize = 1.0f;        // The minimum size multiplier (multiples of the projectile's base size)
    public float endSize = 1.5f;        // The maximum size multiplier (multiples of the projectile's base size)
    public float timeToMax = 2.0f;      // The time to elapse between the projectile going from the minimum to the maximum

    private float startTime = 0.0f;        // The starting time, in seconds
    private Vector3 baseScale;      // The recorded base scale of the projectile

    public override void ProjectileStart()
    {
        // Initialize variables
        baseScale = gameObject.transform.localScale;

        if (enabled)
        {
            Resize(startSize);
        }
    }

    public override void ProjectileUpdate()
    {
        if (enabled)
        {
            if (startTime == 0.0f)
            {
                startTime = Time.time;
            }

            float diffTime = Time.time - startTime;
            Resize(Mathf.Lerp(startSize, endSize, diffTime / timeToMax));
        }
        
    }

    void Resize(float sizeMult)
    {
        gameObject.transform.localScale = baseScale * sizeMult;
        gameObject.transform.localScale.Set(gameObject.transform.localScale.x, gameObject.transform.localScale.y, baseScale.z);
    }
}
