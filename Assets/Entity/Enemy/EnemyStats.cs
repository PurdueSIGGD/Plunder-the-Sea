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
    public int[] dropTable = { -1, 0, 1, 2, 3 };
    public float[] dropOdds = { 1, 1, 1, 1, 1 };
    public float dropRange;
    private bool dead = false;
    public bool elite = false; // Determines if this enemy is an elite enemy.
    public int eliteRank = 0; // Determines how many additional effects this enemy gets when elite.

    private void Start()
    {
        spawnPoint = this.transform.position;
        myBase = GetComponent<EnemyBase>();

        dropRange = 0;
        for (int i = 0; i < dropOdds.Length; i++)
        {
            dropRange += dropOdds[i];
        }
    }

    private void Update()
    {
        StatUpdate();
    }

    public void enemyDamageReturnCall()
    {
        FindObjectOfType<PlayerClasses>().enemyHit(this);
    }

    public override void Die()
    {
        //only die once
        if (dead)
        {
            return;
        }
        dead = true;

        //death call to player
        PlayerClasses pClass = FindObjectOfType<PlayerClasses>();
        pClass.enemyKilled();

        //Randomly select a bait from dropTable based on dropOdds and give to player
        PlayerStats pStats = pClass.GetComponentInParent<PlayerStats>();
        float dropValue = Random.Range(0, dropRange);
        int dropIndex = 0;
        if (dropValue > 0)
        {
            dropIndex = -1;
            while (dropValue > 0)
            {
                dropIndex++;
                dropValue -= dropOdds[dropIndex];
            }
        }
        if (dropTable[dropIndex] >= 0)
        {
            pStats.baitInventory.addBait(dropTable[dropIndex]);
        }
        

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
