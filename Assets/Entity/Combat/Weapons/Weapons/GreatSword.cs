public class GreatSword : Weapon
{   
    static GreatSword() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.GREAT_SWORD, () => new GreatSword());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(5f, 1),
            new SwingProjSys(90f, 0.5f)
        };
}