using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerFishing))]
public class PlayerBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public PlayerFishing fishing;

    private UI_Camera cam;
    
    public void moveHere(Transform newPos)
    {
        this.transform.position = newPos.position;
    }

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 1)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerBase>().moveHere(this.transform);
            }
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        fishing = GetComponent<PlayerFishing>();

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    private void Update()
    {
        var inv = GetComponent<WeaponInventory>();
        

        if (Input.GetMouseButton(0))
        {
            ShootAt(cam.GetMousePosition(), inv.GetRanged(), inv.GetWeaponStats(false));
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShootAt(cam.GetMousePosition(), inv.GetMelee(), inv.GetWeaponStats(true));
        }
    }

    //PlayerStats calls this when player kills entity
    public void OnKill (EntityStats victim) 
    {
        
    }

    public bool ShootAt(Vector2 position, Weapon weapon, WeaponBaseStats stats)
    {
        if (weapon?.CanShoot(this.gameObject) == true)
        {
            Debug.Log(weapon.GetType().FullName);
            Projectile hitbox = Projectile.Shoot(stats.projectilePrefab, this.gameObject, position);
            hitbox.weapon = weapon;
            hitbox.damage = stats.damage;
            hitbox.lifeTime = stats.lifeTime;
            weapon.OnFire(hitbox);
            return true;
        }
        return false;
    }
}
