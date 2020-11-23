using UnityEngine;


[CreateAssetMenu(fileName = "FloatWeaponClassTable", menuName = "ScriptableObjects/Table/FloatWeaponClass", order = 1)]
public class FloatWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Pair
    {
        public WeaponFactory.CLASS name;
        public float floatValue;
    }
    public Pair[] classFloatPairs;
    public float get(WeaponFactory.CLASS weaponClass) {
        return System.Array.Find(this.classFloatPairs, (p) => p.name == weaponClass).floatValue;
    }
}
