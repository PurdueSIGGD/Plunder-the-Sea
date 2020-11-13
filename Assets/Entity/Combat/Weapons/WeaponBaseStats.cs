using UnityEngine;

[System.Serializable]
public class WeaponBaseStats {
    public bool isMelee = false;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public int ammoPerKill = 1;
    public int maxAmmo = 0;
    public int ammo = 0;
    public int damage;
    public float lifeTime;
    public Sprite gunSprite = null;
}