public class BigAxeWeapon : Weapon
{   
    public override CLASS Class() 
        => CLASS.BIG_AXE;

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            new SwingProjSys(90f, 0.5f),
            Weapon.MakeMelee(5f)
        };
}