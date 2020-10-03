using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    public GameObject bobberPrefab;
    public float castingSpeed = 4.0f;
    //Maximum distance before line breaks
    public float castingDistance = 7.0f;
    private Bobber bobber;
    private UI_Camera cam;

    private void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cast Fishing Pole"))//F key
        {
            if (bobber)
            {
                bobber.Reel();
            }
            else
            {
                bobber = Bobber.Create(bobberPrefab, this, cam.GetMousePosition());
            }
        }
    }

    //Called when bobber returns or catches fish
    public void OnReelFinish(Fish fish)
    {
        if (fish)
        {
            Debug.Log("Fish caught");
        }
        else
        {
            Debug.Log("Bobber returned");
        }
    }

}
