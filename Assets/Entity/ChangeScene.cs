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
            //reset ammo and stamina
            PlayerBase player = FindObjectOfType<PlayerBase>();
            PlayerStats stats = player.stats;
            stats.resetAmmo(stats.maxAmmo);
            stats.stamina = stats.staminaMax;

            if (newScene == "Combat")
            {
                FindObjectOfType<PlayerStats>().dungeonLevel++;
            }
            
            SceneManager.LoadScene(newScene);
            if (newScene == "FishPond")
            {
                //Debug.Log("Fish time");
                if (stats.appliedStats != null)
                {
                    Fish.UnbuffPlayerStats(player);
                }
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
