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
                Debug.Log(enemy.name);
                enemy.GetComponent<EnemyStats>().elite = true;
                enemy.GetComponent<EnemyStats>().eliteRank = Random.Range(0,4);
            }
            spawnNum++;
        }
    }

    public float GetEliteChance(int level)
    {
        return 0.1f / (1 + Mathf.Exp(-0.25f * (level - 20)));
    }
}
