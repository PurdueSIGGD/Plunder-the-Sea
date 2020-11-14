public class Harpoon : Weapon
{   
    static Harpoon() => 
        WeaponFactory.BindWeaponClass(
            WeaponFactory.CLASS.HARPOON, () => new Harpoon());

    public override ProjectileSystem[] ConstructSystems() 
        => new ProjectileSystem[]{
            base.MakeRanged(0.3f, 6f, 4),
            new HookProjSys()
        };
}