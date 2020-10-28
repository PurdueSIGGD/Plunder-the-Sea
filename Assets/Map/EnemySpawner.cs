using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies; //array of enemies that can be spawned, add weighting by duplicating the same enemy multiple times to spawn it more often
    public float chanceToSpawn;

    public void spawnEnemies()
    {
        if (UnityEngine.Random.value <= chanceToSpawn) {
            Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
        }
    }
}
