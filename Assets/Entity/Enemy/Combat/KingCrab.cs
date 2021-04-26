using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCrab : StateCombat
{
    public GameObject gate;
    public Vector3 gatePos;

    const int cooldown = (int)ApproachMovement.ApproachState.cooldown;
    const int activating = (int)ApproachMovement.ApproachState.activating;
    const int approaching = (int)ApproachMovement.ApproachState.approaching;

    public float playerDebuffTime = 5f;
    public float playerDebuffAmount = 5f;
    public float enemyDebuffTime = 5f;
    public float enemyDebuffAmount = 5f;

    float playerDebuffTimer = 0.0f;
    float originalPlayerSpeed = 0.0f;

    public EntityAttribute speedDebuff;

    List<Debuff> debuffedEnemies = new List<Debuff>();

    bool charging = false;

    public override void CombatStart()
    {
        base.CombatStart();
        speedDebuff = new EntityAttribute(ENT_ATTR.MOVESPEED, 0.5f, playerDebuffTime, false, false, "King's Slam");
        gatePos = transform.position;
    }

    // Spawn gate on death
    public override void OnDeath()
    {
        base.OnDeath();
        Instantiate(gate, gatePos, Quaternion.identity);
    }

    void Update()
    {
        int current = GetState();

        if (current == activating && charging == false) {
            charging = true;
            Debug.Log("Charging up attack!");
            anim.SetInteger("State", 1);
        }
        if (current == cooldown) {
            if (charging == true) {
                charging = false;
                smash();
            }
        }
        if (current == approaching)
        {
            anim.SetInteger("State", 0);
        }
        ////check if playerDebuffTimer has run out
        //if (playerDebuffTimer != 0 && OnTarget(playerDebuffTimer)) {
        //    PlayerStats playerStats = myBase.player.GetComponent<PlayerStats>();
        //    playerStats.movementSpeed = originalPlayerSpeed;
        //    playerDebuffTimer = 0.0f;
        //}
        ////check if any debuffs have run out or if any enemies have died
        //for(int i = 0; i < debuffedEnemies.Count; i++) {
        //    Debuff debuff = debuffedEnemies[i];
        //    if (debuff.gameObject == null || OnTarget(debuff.time)) {
        //        //if enemy is still alive then set damage back to normal
        //        if (debuff.gameObject != null) {
        //            EnemyStats enemyStats = debuff.gameObject.GetComponent<EnemyStats>();
        //            enemyStats.damage = debuff.originalDamage;
        //        }
        //        //remove debuff 
        //        debuffedEnemies.RemoveAt(i);
        //        i--;
        //    }
        //}

        
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void smash() {
        //do smash animation (TODO)
        //deal damage to player if in range
        Debug.Log("SMASH!");
        if (Vector3.Distance(transform.position, myBase.player.transform.position) <= attackRange)
        {
            //deal damage
            myBase.player.GetComponent<PlayerBase>().stats.TakeDamage(myBase.myStats.damage, myBase.myStats);
            //debuff player
            myBase.player.GetComponent<PlayerStats>().AddAttribute(speedDebuff, myBase.myStats);
            //if (playerDebuffTimer == 0.0f) {
            //    PlayerStats playerStats = myBase.player.GetComponent<PlayerStats>();
            //    originalPlayerSpeed = playerStats.movementSpeed;
            //    playerStats.movementSpeed -= playerDebuffAmount;
            //    playerDebuffTimer = SetTarget(playerDebuffTime);
            //}
        }
        
        //debff enemies in range
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            if (Vector3.Distance(enemy.transform.position, transform.position) > attackRange) {
                continue;
            }

            if (enemy.Equals(this.gameObject))
            {
                continue;
            }
            ////check if enemy is in debuffed enemies
            //bool inDebuffedEnemies = false;
            //for(int i = 0; i < debuffedEnemies.Count; i++) {
            //    Debuff debuff = debuffedEnemies[i];
            //    //if enemy is dead then remove debuff and continue
            //    if (debuff.gameObject == null) {
            //        debuffedEnemies.RemoveAt(i);
            //        i--;
            //        continue;
            //    }
            //    //if enemy is in debuffed enemies then update debuff
            //    if (debuff.gameObject.Equals(enemy)) {
            //        inDebuffedEnemies = true;
            //        debuff.time = SetTarget(enemyDebuffTime);
            //        //don't currently stack debuffs although this could change later
            //        break;
            //    }
            //}
            ////if enemy is not in debuffed enemies then add it and debuff the enemy
            //if (!inDebuffedEnemies) {
            //    EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
            //    Debuff newDebuffedEnemy = new Debuff(SetTarget(enemyDebuffTime), enemyStats.damage, enemy);
            //    enemyStats.damage -= enemyDebuffAmount;
            //    debuffedEnemies.Add(newDebuffedEnemy);
            //}
            enemy.GetComponent<EnemyStats>().AddAttribute(speedDebuff, myBase.myStats);
        }
        

    }
   
}

struct Debuff {
    public float time;
    public float originalDamage;
    public GameObject gameObject;
    
    public Debuff(float time, float originalDamage, GameObject gameObject) {
        this.time = time;
        this.originalDamage = originalDamage;
        this.gameObject = gameObject;
    }
}