using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChange : ProjectileModifier
{
    // A projectile with SizeChange can change sizes as they exist, either growing, or shrinking.

    public float startSize = 1.0f;        // The minimum size multiplier (multiples of the projectile's base size)
    public float endSize = 1.5f;        // The maximum size multiplier (multiples of the projectile's base size)
    public float timeToMax = 2.0f;      // The time to elapse between the projectile going from the minimum to the maximum

    private float startTime;        // The starting time, in seconds
    private float sizeSlope;        // The slope of the size change, calculated by using rise over run (tm)
    private Vector3 baseScale;      // The recorded base scale of the projectile

    public override void ProjectileStart()
    {
        // Initialize variables
        startTime = Time.time;
        baseScale = gameObject.transform.localScale;
        sizeSlope = (endSize - startSize) / (timeToMax);

        gameObject.transform.localScale = baseScale *= startSize;
    }

    public override void ProjectileUpdate()
    {
        if (Time.time > startTime+timeToMax)
        {
            gameObject.transform.localScale = baseScale * endSize;
        } else if (Time.time < startTime)
        {
            gameObject.transform.localScale = baseScale * startSize;
        } else
        {
            float diffTime = Time.time - startTime;
            gameObject.transform.localScale = baseScale * (startSize + sizeSlope * diffTime);
        }
    }
}
