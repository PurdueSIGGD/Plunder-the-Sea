public class Cutlass : Weapon
{   
    static Cutlass() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.CUTLASS, () => new Cutlass());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeMelee(1f, 1),
            new SwingProjSys(60f, 0f)
        };
}