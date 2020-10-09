using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    public PlayerBase player;
    public GameObject bobberPrefab;
    public float castingSpeed = 4.0f;
    //Maximum distance before line breaks
    public float castingDistance = 7.0f;
    private Bobber bobber;
    private UI_Camera cam;

    private int selectedBait = 0;
    private const int amountOfBaitTypes = 2;

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        player = GetComponent<PlayerBase>();
        player.stats.addBait(0); //just for testing
        player.stats.addBait(1); //just for testing
    }

    private void Update()
    {
        if (Input.GetButtonDown("Toggle Bait"))//B key
        {
            selectedBait = (selectedBait + 1) % amountOfBaitTypes;
            Debug.Log("Bait " + selectedBait + " selected");
        }
        
        // NEED TO REMOVE THIS AT SOME POINT!!!
        if (Input.GetKey(KeyCode.Alpha1))//1 button
        {
            player.stats.addBait(0); //just for testing bait
        }

        if (Input.GetButtonDown("Cast Fishing Pole"))//F key
        {
            if (bobber)
            {
                bobber.Reel();
            }
            else
            {
                if (player.stats.getBaitArray()[selectedBait] > 0)
                {
                    bobber = Bobber.Create(bobberPrefab, this, cam.GetMousePosition(), selectedBait);
                    player.stats.removeBait(selectedBait);
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
