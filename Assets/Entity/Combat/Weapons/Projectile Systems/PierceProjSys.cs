using System;
using UnityEngine;

public class PierceProjSys : ProjectileSystem {

    /* Amount of targets allowed to hit */
    public int pierceAmount;

    protected override void OnEquip(WeaponInventory inv) {
        this.pierceAmount = this.tables.pierce.get(this.weaponClass).Value;
    }
    public override void OnHit(Projectile proj, EntityStats victim)
    {
        proj.destroyOnCollide = (proj.pierceCount >= pierceAmount);
    }

    // only weapons with the "pierce" stat can use this system
    static PierceProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.pierce.get(c).HasValue) ? new PierceProjSys() : null
        );
}