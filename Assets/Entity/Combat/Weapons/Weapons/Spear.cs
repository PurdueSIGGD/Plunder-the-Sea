public class Spear : Weapon
{   
    static Spear() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.SPEAR, () => new Spear());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(15f, 1),
            new ThrustProjSys()
        };
}