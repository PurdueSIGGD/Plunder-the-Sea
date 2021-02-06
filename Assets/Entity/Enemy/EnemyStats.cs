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
    public int[,] dropTable = { { -1,0,1,2,3 },
                                 {  1,1,1,1,1 } };

    private void Start()
    {
        spawnPoint = this.transform.position;
        myBase = GetComponent<EnemyBase>();
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
        //death call to player
        PlayerClasses pClass = FindObjectOfType<PlayerClasses>();
        pClass.enemyKilled();

        //Randomly select a bait or nothing from dropTable and give to player
        PlayerStats pStats = pClass.GetComponentInParent<PlayerStats>();
        int dropIndex = Random.Range(0, dropTable[0].Length);
        if (dropTable[0,dropIndex] >= 0)
        {
            pStats.addBait(dropTable[0,dropIndex]);
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
