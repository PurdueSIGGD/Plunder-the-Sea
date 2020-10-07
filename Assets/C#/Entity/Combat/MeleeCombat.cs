using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    PlayerBase pbase;

    public GameObject projectilePrefab;
    /* Cooldown length in seconds */
    public float projectileCooldown = 0.3f;
    public float timeSinceLastHit = 0;
    public float meleeOffset;
    public int staminaMax = 100;
    public float stamina = 100f;
    public int staminaCost = 15;
    public float staminaRechargeRate = 2f;

    public int staminaIndicator;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
    }

    public void Update()
    {
        staminaIndicator = (int)GetStamina();
    }

    public bool CanShoot()
    {
        return stamina >= staminaCost;
    }
    
    public float GetStamina()
    {
        return Mathf.Min(stamina + (float)((Time.time - timeSinceLastHit) * staminaRechargeRate), staminaMax);
    }

    /* Returns true if projectile was shot */
    public bool ShootAt(Vector2 position)
    {
        stamina = pbase.stats.stamina;
        if (CanShoot())
        {
            Projectile hitbox = Projectile.Shoot(projectilePrefab, gameObject, position, 0f);
            hitbox.destroyOnCollide = false;
            hitbox.transform.SetParent(this.gameObject.transform);
            timeSinceLastHit = Time.time;
            pbase.stats.UseStamina(staminaCost);
            return true;
        }
        return false;

    }
}
