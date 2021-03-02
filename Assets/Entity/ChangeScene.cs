using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string newScene = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (newScene == "Combat")
            {
                FindObjectOfType<PlayerStats>().dungeonLevel++;
            }
            else
            {
                PlayerBase player = FindObjectOfType<PlayerBase>();
                if (player.stats.appliedFish != null)
                {
                    player.stats.appliedFish.UnbuffPlayerStats(player);
                }
            }
            SceneManager.LoadScene(newScene);
        }
    }
}
