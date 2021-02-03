using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "IntWeaponClassTable", menuName = "ScriptableObjects/Table/IntWeaponClass", order = 1)]
public class IntWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Pair
    {
        public WeaponFactory.CLASS weaponClass;
        public int integer;
    }
    public Pair[] classIntPairs;

    public int? get(WeaponFactory.CLASS weaponClass) {
        return System.Array.Find(classIntPairs, (p) => p.weaponClass == weaponClass)?.integer;
    }

    [ContextMenu("generate defaults")]
    private void generateDefaults() {
        this.classIntPairs = 
            (System.Enum.GetValues(typeof(WeaponFactory.CLASS)) as WeaponFactory.CLASS[])
            .Select((c) => new Pair(){weaponClass = c, integer = 0}).ToArray();
    }
} 

