using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    EnemyBase myBase;
    public Vector3 spawnPoint;
    public float attackSpeedInverse = 10;
    public float damage = 1.5f;
    public float numberOfTimesToRespawn = 0; // Enemies do not respawn by default.

    private void Start()
    {
        spawnPoint = this.transform.position;
        myBase = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        StatUpdate();
    }

    public override void Die()
    {
        myBase.myCombat.OnDeath();
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
