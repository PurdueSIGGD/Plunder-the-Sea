using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class debugging : MonoBehaviour
{

    [SerializeField]
    private bool debug = false;
    private PlayerStats stats;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            debugChecks();
        }
    }

    private void debugChecks()
    {
        //reloads this scene
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //teleports to next dungeon level
        if (Input.GetKeyDown(KeyCode.Return))
        {
            stats.resetAmmo(stats.maxAmmo);
            stats.stamina = stats.staminaMax;
            stats.dungeonLevel++;
            SceneManager.LoadScene("Combat");
        }
        //teleports to previous dungeon level
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (stats.dungeonLevel > 0)
            {
                stats.dungeonLevel += 1;
                stats.resetAmmo(stats.maxAmmo);
                stats.stamina = stats.staminaMax;
                stats.dungeonLevel--;
            }
            SceneManager.LoadScene("Combat");
        }
    }
}
