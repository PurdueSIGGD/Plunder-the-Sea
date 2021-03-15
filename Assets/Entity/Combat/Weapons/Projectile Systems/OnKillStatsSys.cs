public class OnKillStatsSys : ProjectileSystem {

    private int ammoPerKill;
    private PlayerStats stats;

    public override void OnKill(EntityStats victim)
    {
        //this.stats.replenishAmmo(this.ammoPerKill);
    }

    protected override void OnEquip(WeaponInventory inv)
    {
        this.ammoPerKill = this.tables.ammoPerKill.get(this.weaponClass).Value;
        this.stats = inv.GetComponent<PlayerBase>().stats;
    }

    
    // only weapons with the ammoPerKill stat can use this system
    static OnKillStatsSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.ammoPerKill.get(c).HasValue) ? new OnKillStatsSys() : null
        );
}