using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
public static class WeaponFactory {

    private static List<Func<CLASS, WeaponTables, ProjectileSystem>> makeSystemMethods 
        = new List<Func<CLASS, WeaponTables, ProjectileSystem>>();

    public static void BindSystem(Func<CLASS, WeaponTables, ProjectileSystem> makeMethod) {
        WeaponFactory.makeSystemMethods.Add(makeMethod);
    }
    // Ensures that the weapon sub classes have their static constructors ran
    // same thing for projectile systems
    static WeaponFactory() {
        AppDomain.CurrentDomain.GetAssemblies().ToList()
        .ForEach(assem => assem.GetTypes()
            .Where(st => st.BaseType == typeof(ProjectileSystem)).ToList()
            .ForEach(st => RuntimeHelpers.RunClassConstructor(st.TypeHandle))
        );
    }

    public enum CLASS {
        // Melee
        BIG_AXE, BLUNDER_BUSS, BOTTLE, CROSSBOLT, CUTLASS, 
        DAGGER,
        // Ranged
        DUALIES, DUCKSHOT, FLINTLOCK, GREAT_SWORD, 
        HARPOON, SPEAR, SQUIDGUN, VOLLEYGUN, WHIP
    }

    public enum TAG {
        MELEE, RANGED
    }

    public static ProjectileSystem[] DeriveSystems(WeaponFactory.CLASS weaponClass, WeaponTables tables) {
        var systems = new List<ProjectileSystem>();

        foreach (var makeMethod in WeaponFactory.makeSystemMethods)
        {
            var system = makeMethod(weaponClass, tables);

            if (system != null) {
                systems.Add(system);
            }
        }

        return systems.ToArray();
    }
}