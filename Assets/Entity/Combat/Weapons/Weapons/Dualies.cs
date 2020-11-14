public class Dualies : Weapon
{   
    static Dualies() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.DUALIES, () => new Dualies());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5),
            new MultishotProjSys(1, 180f)
        };
}