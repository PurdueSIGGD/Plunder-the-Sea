using UnityEngine;

public class SwingProjSys : ProjectileSystem
{
    private float sweepAngle;
    private float holdTime;

    protected override void OnEquip(WeaponInventory inv)
    {
        this.sweepAngle = this.tables.swingAngle.get(this.weaponClass).Value;
        this.holdTime = this.tables.swingHold.get(this.weaponClass).Value;
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

    
    // only weapons with the swing stats can use this system
    static SwingProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.swingAngle.get(c).HasValue && t.swingHold.get(c).HasValue) ? new SwingProjSys() : null
        );
}