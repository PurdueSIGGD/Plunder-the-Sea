public class Volleygun : Weapon
{   
    static Volleygun() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.VOLLEYGUN, () => new Volleygun());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5),
            new BurstfireProjSys(3, 0.05f)
        };
}