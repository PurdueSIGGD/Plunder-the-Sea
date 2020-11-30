using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    [SerializeField]
    private GameObject PopupText;

    public PlayerBase player;
    public GameObject bobberPrefab;
    public float castingSpeed = 4.0f;
    //Maximum distance before line breaks
    public float castingDistance = 7.0f;
    public AudioClip castSound;
    public AudioClip reelSound;
    private Bobber bobber;
    private UI_Camera cam;
    private AudioSource audioSrc;
    private bool bobberIsCast = false;

    private int selectedBait = 0;
    private const int amountOfBaitTypes = 2;

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
        audioSrc = GetComponent<AudioSource>();
        if (!audioSrc)
        {
            audioSrc = this.gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Toggle Bait") && !bobberIsCast)//B key
        {
            selectedBait = (selectedBait + 1) % amountOfBaitTypes;
            Debug.Log("Bait " + (selectedBait + 1) + " selected");
            player.stats.changeRedText(selectedBait);
        }
        
        // NEED TO REMOVE THIS AT SOME POINT!!!
        if (Input.GetKeyDown(KeyCode.Alpha1))//1 button
        {
            player.stats.addBait(selectedBait); //just for testing bait
        }

        if (Input.GetButtonDown("Cast Fishing Pole"))//F key
        {
            if (bobber)
            {
                audioSrc.clip = reelSound;
                audioSrc.loop = true;
                audioSrc.Play();
                bobber.Reel();
            }
            else
            {
                if (player.stats.getBaitArray()[selectedBait] > 0)
                {
                    audioSrc.clip = castSound;
                    audioSrc.Play();
                    bobber = Bobber.Create(bobberPrefab, this, cam.GetMousePosition(), selectedBait);
                    bobberIsCast = true;
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
        audioSrc.loop = false;
        audioSrc.Stop();
        if (fish)
        {
            Debug.Log("Fish caught");
            fish.FishingMinigame.SetActive(true);
        }
        else
        {
            Debug.Log("Bobber returned");
            player.stats.addBait(selectedBait);
        }
        bobberIsCast = false;
    }

    public void SpawnPopupText(string text)
    {
        GameObject textObject = Instantiate(PopupText, transform.position, Quaternion.identity);
        textObject.transform.position = new Vector3(textObject.transform.position.x, textObject.transform.position.y + 0.3f, 0);
        TextMesh textMesh = textObject.GetComponent<TextMesh>();
        textMesh.text = text;
    }

}
