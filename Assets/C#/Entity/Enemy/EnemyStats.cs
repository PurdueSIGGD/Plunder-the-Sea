using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    public Vector3 spawnPoint;
    public float attackSpeedInverse = 10;
    public float damage = 1.5f;
    public float numberOfTimesToRespawn = 0; // Enemies do not respawn by default.
    private RangedCombat PlayerGun;

    private void Start()
    {
        spawnPoint = this.transform.position;
        PlayerGun = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent(typeof(RangedCombat)) as RangedCombat;
    }

    public override void Die()
    {
        PlayerGun.addAmmo(); //Refill ammo for kill
        if(numberOfTimesToRespawn == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (numberOfTimesToRespawn != -1) 
            { 
                numberOfTimesToRespawn--;
            }
            this.transform.position = spawnPoint;
            currentHP = maxHP;
            healthbar.value = 1;
        }
    }
}
