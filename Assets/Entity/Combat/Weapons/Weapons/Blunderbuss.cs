public class Blunderbuss : Weapon
{   
    static Blunderbuss() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.BLUNDER_BUSS, () => new Blunderbuss());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 7f, 5)
        };
}