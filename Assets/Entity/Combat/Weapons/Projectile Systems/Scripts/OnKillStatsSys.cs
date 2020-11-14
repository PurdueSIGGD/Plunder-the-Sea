public class OnKillStatsSys : ProjectileSystem {

    private int ammoPerKill;

    private PlayerStats stats;

    public OnKillStatsSys(int ammoPerKill) {
        this.ammoPerKill = ammoPerKill;
    }

    public override void OnKill(EntityStats victim)
    {
        this.stats.replenishAmmo(this.ammoPerKill);
    }   

    
}