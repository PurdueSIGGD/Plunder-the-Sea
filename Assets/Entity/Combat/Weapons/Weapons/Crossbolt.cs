public class Crossbolt : Weapon
{   
    static Crossbolt() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.CROSSBOLT, () => new Crossbolt());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5),
            new PierceProjSys(3)
        };
}