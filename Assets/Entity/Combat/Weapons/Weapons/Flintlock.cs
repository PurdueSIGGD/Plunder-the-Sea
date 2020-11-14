public class Flintlock : Weapon
{   
    static Flintlock() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.FLINTLOCK, () => new Flintlock());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5),
        };
}