using UnityEngine;

public class SwingProjSys : ProjectileSystem
{
    private float sweepAngle;
    private float holdTime;

    public SwingProjSys(float sweepAngle, float holdTime) {
        this.sweepAngle = sweepAngle;
        this.holdTime = holdTime;
    }

    public override void Run(Projectile projectile)
    {
        if (projectile.currentLifeTime > holdTime)
        {
            projectile.transform.Rotate(0, 0, Time.deltaTime * sweepAngle / (projectile.lifeTime - holdTime));
        }
    }

    public override void OnFire(Projectile projectile)
    {
        projectile.transform.Rotate(0, 0, -sweepAngle / 2);
    }

}