using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "TagWeaponClassTable", menuName = "ScriptableObjects/Table/TagWeaponClass", order = 1)]
public class TagWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Pair
    {
        public WeaponFactory.CLASS name;
        public WeaponFactory.TAG tag;
    }
    public Pair[] pairs;

    public WeaponFactory.TAG get(WeaponFactory.CLASS name) {
        return System.Array.Find(pairs, (p) => p.name == name).tag;
    }

    [ContextMenu("generate defaults")]
    private void generateDefaults() {
        this.pairs = 
            (System.Enum.GetValues(typeof(WeaponFactory.CLASS)) as WeaponFactory.CLASS[])
            .Select((c) => new Pair(){name = c, tag = WeaponFactory.TAG.RANGED }).ToArray();
    }
} 