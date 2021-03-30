using UnityEngine;


[CreateAssetMenu(fileName = "DescrWeaponClassTable", menuName = "ScriptableObjects/Table/DescrWeaponClass", order = 1)]
public class DescrWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Pair
    {
        public WeaponFactory.CLASS weaponClass;
        [TextArea]
        public string description;
    }
    public Pair[] classDescrPairs;
    public string get(WeaponFactory.CLASS weaponClass)
    {
        return System.Array.Find(this.classDescrPairs, (p) => p.weaponClass == weaponClass).description;
    }
}
