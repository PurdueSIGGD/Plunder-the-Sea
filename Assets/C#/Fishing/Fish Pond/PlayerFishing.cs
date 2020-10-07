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

    private int selectedBait;

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        playerStats = FindObjectOfType<PlayerStats>();
        playerStats.addIOneBat(); //just for testing
        playerStats.addIZeroBait(); //just for testing
    }

    private void Update()
    {
        //maybe change this to a button to switch at a later date
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selectedBait = 0;
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selectedBait = 1;
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
                    Debug.Log("None of selected bait "+selectedBait.ToString()+" left");
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
