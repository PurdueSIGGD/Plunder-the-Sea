using UnityEngine;


[CreateAssetMenu(fileName = "PrefabWeaponClassTable", menuName = "ScriptableObjects/Table/PrefabWeaponClass", order = 1)]
public class PrefabWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Pair
    {
        public WeaponFactory.CLASS weaponClass;
        public GameObject prefab;
    }
    public Pair[] classPrefabPairs;
    public GameObject get(WeaponFactory.CLASS weaponClass) {
        return System.Array.Find(this.classPrefabPairs, (p) => p.weaponClass == weaponClass).prefab;
    }
}
