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
            Instantiate(enemiesToSpawn[i], spawnPoints[spawnNum].transform.position, Quaternion.identity);
            spawnNum++;
        }
    }
}
