using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathButton : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
    }

    public void loadScene(string sceneName)
    {
        // Load the scene and reset player stats
        PlayerClasses.resetDeathStats();
        SceneManager.LoadScene(sceneName);
    }
}
