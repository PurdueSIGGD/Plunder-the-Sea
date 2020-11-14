public class Whip : Weapon
{   
    static Whip() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.WHIP, () => new Whip());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(0.3f, 1),
            new WhipProjSys(0.03f)
        };
}