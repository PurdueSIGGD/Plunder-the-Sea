using UnityEngine;


[CreateAssetMenu(fileName = "DescrWeaponClassTable", menuName = "ScriptableObjects/Table/DescrWeaponClass", order = 1)]
public class DescrWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Trio
    {
        public WeaponFactory.CLASS weaponClass;
        public string name;
        [TextArea]
        public string description;
    }
    public Trio[] classDescrTrios;
    public string getDescr(WeaponFactory.CLASS weaponClass)
    {
        return System.Array.Find(this.classDescrTrios, (p) => p.weaponClass == weaponClass).description;
    }

    public string getName(WeaponFactory.CLASS weaponClass)
    {
        return System.Array.Find(this.classDescrTrios, (p) => p.weaponClass == weaponClass).name;
    }
}
