using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    public GameObject bobberPrefab;
    public float castingSpeed = 4.0f;
    //Maximum distance before line breaks
    public float castingDistance = 7.0f;
    private Bobber bobber;
    private UI_Camera cam;
    private PlayerStats playerStats;

    private int selectedBait = 0;

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        playerStats = FindObjectOfType<PlayerStats>();
        playerStats.addBait(0); //just for testing
        playerStats.addBait(1); //just for testing
    }

    private void Update()
    {
        //maybe change this to a button to switch at a later date
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selectedBait = 0;
            Debug.Log("Bait 1 selected");
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            selectedBait = 1;
            Debug.Log("Bait 2 selected");
        }

        if (Input.GetButtonDown("Cast Fishing Pole"))//F key
        {
            if (bobber)
            {
                bobber.Reel();
            }
            else
            {
                if (playerStats.getBaitArray()[selectedBait] > 0)
                {
                    bobber = Bobber.Create(bobberPrefab, this, cam.GetMousePosition(), selectedBait);
                    playerStats.removeBait(selectedBait);
                }
                else
                {
                    Debug.Log("None of selected bait "+(selectedBait+1).ToString()+" left");
                }
            }
        }
    }

    //Called when bobber returns or catches fish
    public void OnReelFinish(Fish fish)
    {
        if (fish)
        {
            Debug.Log("Fish caught");
            SceneManager.LoadScene("FishingMinigame");
        }
        else
        {
            Debug.Log("Bobber returned");
        }
    }

}
