using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "HookProjSys", menuName = "ScriptableObjects/ProjectileSystems/Hook", order = 1)]
public class HookProjSys : ProjectileSystem {

    private bool retracting = false;
    private Projectile currentHook = null;
    private EntityStats target = null;

    public override void OnFire(Projectile projectile)
    {
        if (currentHook)
        {
            currentHook.Destroy();
        }
        currentHook = projectile;
        retracting = false;
        projectile.destroyOnCollide = false;
    }

    public override void OnHit(Projectile proj, EntityStats victim)
    {
        if (!target)
        {
            target = victim;
            Retract();
        }
    }

    public override void Run(Projectile projectile)
    {
        if (currentHook)
        {
            //Time to retract
            if (!retracting && currentHook.currentLifeTime >= currentHook.lifeTime / 2.0f)
            {
                Retract();
            }
            //Pull target
            if (target)
            {
                target.transform.position = currentHook.transform.position;
            }
        }
    }

    public override void OnEnd(Projectile projectile)
    {
        target = null;
    }

    /* Start pulling hook back */
    private void Retract()
    {
        if (!retracting && currentHook)
        {
            retracting = true;
            Rigidbody2D rigid = currentHook.GetComponent<Rigidbody2D>();
            rigid.velocity *= -1;
            //Set lifetime to destroy hook when it returns
            currentHook.lifeTime = currentHook.currentLifeTime * 2.0f;
        }
    }

    // only a select few weapons can use this system
    static HookProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (c == WeaponFactory.CLASS.HARPOON) ? new HookProjSys() : null
        );
}