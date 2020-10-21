using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;

    public void spawnEnemies()
    {
        //I way trying to make enemy spawning randomized based on chance, but the values was not being stored for some reason
        //if (UnityEngine.Random.value <= chanceToSpawn) {
            Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
        //}
    }
}
