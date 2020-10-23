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
            ShootAt(cam.GetMousePosition(), inv.GetRanged());
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShootAt(cam.GetMousePosition(), inv.GetMelee());
        }
    }

    //PlayerStats calls this when player kills entity
    public void OnKill (EntityStats victim) 
    {
        
    }

    public bool ShootAt(Vector2 position, ScriptableWeapon weapon)
    {
        if (weapon.CanShoot(this.gameObject))
        {
            Projectile hitbox = Projectile.Shoot(weapon.projectilePrefab, this.gameObject, position, weapon.projectileSpeed);
            hitbox.weapon = weapon;
            hitbox.damage = weapon.damage;
            hitbox.lifeTime = weapon.lifeTime;
            weapon.OnFire(hitbox);
            return true;
        }
        return false;
    }
}
