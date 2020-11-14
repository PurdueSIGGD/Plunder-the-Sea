public class BigAxe : Weapon
{   
    static BigAxe() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.BIG_AXE, () => new BigAxe());
    
    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(5f, 1),
            new SwingProjSys(90f, 0.5f),
        };
}