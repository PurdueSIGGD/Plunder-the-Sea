using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void changeScene(string sceneName)
    {
        //Debug.Log("ChangeScene");
        SceneManager.LoadScene(sceneName);
    }
}
