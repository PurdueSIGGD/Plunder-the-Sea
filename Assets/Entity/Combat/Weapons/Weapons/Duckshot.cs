public class Duckshot : Weapon
{   
    static Duckshot() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.DUCKSHOT, () => new Duckshot());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5),
            new MultishotProjSys(2, 10f)
        };
}