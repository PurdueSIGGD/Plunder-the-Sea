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
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            Instantiate(enemiesToSpawn[i], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        }
    }
}
