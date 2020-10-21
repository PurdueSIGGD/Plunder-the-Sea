using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;

    public void spawnEnemies()
    {
        Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
    }
}
