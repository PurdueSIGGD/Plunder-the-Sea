public class Bottle : Weapon
{   
    static Bottle() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.BOTTLE, () => new Bottle());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(1f, 1),
            new RevertProjSys(5f)
        };
}