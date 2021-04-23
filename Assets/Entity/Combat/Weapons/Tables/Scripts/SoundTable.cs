using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSoundTable", menuName = "ScriptableObjects/Table/WeaponSoundTable", order = 3)]
public class SoundTable : ScriptableObject
{
    [System.Serializable]
    public class soundPair
    {
        public WeaponFactory.CLASS weaponClass;
        public AudioClip clip;
        public float volume;
        public float pitch;
    }
    public soundPair[] classSounds;

    public AudioClip getSound(WeaponFactory.CLASS weaponClass)
    {
        return System.Array.Find(this.classSounds, (p) => p.weaponClass == weaponClass).clip;
    }
    public float getVolume(WeaponFactory.CLASS weaponClass)
    {
        return System.Array.Find(this.classSounds, (p) => p.weaponClass == weaponClass).volume;
    }
    public float getPitch(WeaponFactory.CLASS weaponClass)
    {
        return System.Array.Find(this.classSounds, (p) => p.weaponClass == weaponClass).pitch;
    }
}
