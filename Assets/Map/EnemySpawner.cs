using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enemyGroup[] groups; //groups of enemies that can spawn
    public GameObject[] spawnPoints;

    public void spawnEnemies()
    {
        int level = FindObjectOfType<PlayerStats>().dungeonLevel;
        GameObject[] enemiesToSpawn = groups[Random.Range(0, groups.Length)].enemySet;
        int spawnNum = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (spawnNum >= spawnPoints.Length)
            {
                spawnNum = 0;
            }
            GameObject enemy = Instantiate(enemiesToSpawn[i], spawnPoints[spawnNum].transform.position, Quaternion.identity);
            if (Random.Range(0f,1f) <= GetEliteChance(level))
            {
                // Make an enemy elite if it succeeds in the check to become elite (with random rank)
                EnemyStats es = enemy.GetComponent<EnemyStats>();
                Debug.Log("Elite " + es.displayName);
                es.elite = true;
                es.eliteRank = Random.Range(0,4);
            }
            spawnNum++;
        }
    }

    // Returns the Elite Chance, starting low, growing for a while and eventually softcapping at 0.1, or 10%.
    public float GetEliteChance(int level)
    {
        return 0.1f / (1 + Mathf.Exp(-0.2f * (level - 20)));
    }
}
