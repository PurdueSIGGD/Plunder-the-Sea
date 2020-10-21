using UnityEngine;

[CreateAssetMenu(fileName = "SwingProjSys", menuName = "ScriptableObjects/ProjectileSystems/Swing", order = 1)]
public class SwingProjSys : ProjectileSystem
{
    public float sweepAngle;
    public float holdTime;

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