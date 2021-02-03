using UnityEngine;


[CreateAssetMenu(fileName = "SpritesWeaponClassTable", menuName = "ScriptableObjects/Table/SpritesWeaponClass", order = 1)]
public class SpritesWeaponClassTable : ScriptableObject
{
    // DO NOT MODIFY THE PAIR CLASS
    [System.Serializable]
    public class Pair
    {
        public WeaponFactory.CLASS weaponClass;
        public Sprite sprite;
    }
    public Pair[] classSpritePairs;
    public Sprite get(WeaponFactory.CLASS weaponClass) {
        return System.Array.Find(this.classSpritePairs, (p) => p.weaponClass == weaponClass).sprite;
    }
}
