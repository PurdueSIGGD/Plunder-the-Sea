public class Dagger : Weapon
{   
    static Dagger() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.DAGGER, () => new Dagger());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(15f, 1),
            new ThrustProjSys()
        };
}