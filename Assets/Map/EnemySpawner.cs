using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies; //array of enemies that can be spawned, add weighting by duplicating the same enemy multiple times to spawn it more often
    public float chanceToSpawn = 1f;

    public void spawnEnemies()
    {
        int enemyNum = FindObjectOfType<PlayerStats>().dungeonLevel;
        chanceToSpawn += enemyNum * 0.1f;
        while(chanceToSpawn > 0) {
            if (UnityEngine.Random.value <= chanceToSpawn) {
                Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
            }
            chanceToSpawn--;
        }
    }
}
