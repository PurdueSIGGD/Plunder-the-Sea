public class Squidgun : Weapon
{   
    static Squidgun() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.SQUIDGUN, () => new Squidgun());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5),
        };
}