using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Retreat from player until it is a certain distance away then do action.
 * TODO:
 * - fix bug where enemies can spawn in walls and places that they shouldn't
 * - make it not spawn enemies if player isn't near by
*/
public class RetreatMovement : StateMovement
{
    public SpriteRenderer sprite;
    public Animator anim;

    //the distance to retreat to before doing action
    public float retreatDistance = 5f;

    //how much time in seconds to wait to do another action after retreating
    public float actCooldownTime = 5.0f;

    //the enemy to spawn
    public GameObject enemyToSpawn;

    //the number of enemies to spawn
    public int numberOfEnemiesToSpawn = 3;

    //won't spawn more than the maximum number of enemies
    public int maxEnemiesToSpawn = 6;

    //the enemies that have been spawned
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public enum RetreatState
    {
        ACTIVATING,
        RETREATING,
        COOLDOWN
    }

    public RetreatState retreatState = RetreatState.ACTIVATING;

    // Update is called once per frame
    void Update()
    {
        //if you arn't near the player then don't do anything
        if (PlayerDistance() > 8) { 
            return;
        }
        //do action based on retreatState 
        switch (retreatState)
        {
            case RetreatState.ACTIVATING:
                anim.SetInteger("State", 1);
                myBase.myRigid.velocity = Vector2.zero;
                doAction();
                retreatState = RetreatState.RETREATING;
                break;
            case RetreatState.RETREATING:
                anim.SetInteger("State", 0);
                if (myBase.player)
                {
                    sprite.flipX = !(myBase.player.transform.position.x < transform.position.x);
                }

                myBase.myStats.currentHP = 1;
                //check if far enough away from the player
                if (PlayerDistance() > retreatDistance)
                {
                    retreatState = RetreatState.COOLDOWN;
                    SetTarget(actCooldownTime);
                }
                else
                {
                    //Debug.Log(PlayerDistance());
                    MoveAway();
                }
                break;
            case RetreatState.COOLDOWN:
                anim.SetInteger("State", 1);

                myBase.myRigid.velocity = Vector2.zero;
                myBase.myStats.currentHP = myBase.myStats.maxHP;
                //wait until cooldown time is over
                if (OnTarget())
                {
                    retreatState = RetreatState.ACTIVATING;
                }
                break;
        }
    }

    /**
     * The action to be done when far enough away from the player. In this 
     * case spawn enemies.
    */
    private void doAction()
    {
        for(int i = 0; i < spawnedEnemies.Count; i++) {
            //if an enemy has been killed then remove it
            if(!spawnedEnemies[i]){
                spawnedEnemies.RemoveAt(i);
            }
        }
        //don't spawn more enemies than maxEnemiesToSpawn
        if (spawnedEnemies.Count > maxEnemiesToSpawn - numberOfEnemiesToSpawn) {
            return;
        }

        Vector3 dirToPlayer = myBase.player.transform.position - transform.position;
        dirToPlayer = dirToPlayer.normalized;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer);

        //Vector3 spawnLocation = transform.position + dirToPlayer.normalized;
        Vector3 spawnLocation = transform.position;
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            //spawnLocation += dirToPlayer;
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnLocation, Quaternion.identity);
            spawnedEnemies.Add(spawnedEnemy);
        }
    }
}
