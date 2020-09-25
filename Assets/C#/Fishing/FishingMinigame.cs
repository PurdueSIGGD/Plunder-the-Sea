using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishingMinigame : MonoBehaviour
{

    private Canvas canvas;
    private UI_Fish fish;

    private void Start()
    {

        canvas = GetComponentInParent<Canvas>();
        fish = GetComponentInChildren<UI_Fish>();

    }

}
