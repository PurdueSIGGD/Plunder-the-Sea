using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
public static class WeaponFactory {
    private static Dictionary<CLASS, Func<Weapon>> makeMethods 
        = new Dictionary<CLASS, Func<Weapon>>();

    public static void BindWeaponClass(CLASS weaponClass, Func<Weapon> makeMethod) {
        WeaponFactory.makeMethods.Add(weaponClass, makeMethod);
    } 
    // Ensures that the weapon sub classes have their static constructors ran
    static WeaponFactory() {
        AppDomain.CurrentDomain.GetAssemblies().ToList()
        .ForEach(assem => assem.GetTypes()
            .Where(st => st.BaseType == typeof(Weapon)).ToList()
            .ForEach(st => RuntimeHelpers.RunClassConstructor(st.TypeHandle))
        );
    }
    public enum CLASS {
        // Melee
        BIG_AXE, BLUNDER_BUSS, BOTTLE, CROSSBOLT, CUTLASS, 
        DAGGER,
        // Ranged
        DUALIES, DUCKSHOT, FLINTLOCK, GREAT_SWORD, 
        HARPOON, RAPIER, SPEAR, SQUIDGUN, VOLLEYGUN, WHIP
    }
    public static Weapon MakeWeapon(WeaponFactory.CLASS weaponClass) {
        return makeMethods[weaponClass]();
    }
}