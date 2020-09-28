using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FishingMinigame : MonoBehaviour
{

    private Canvas canvas;
    private UI_Fish fish;
    private DDR ddr;

    private void Start()
    {

        canvas = GetComponentInParent<Canvas>();
        fish = GetComponentInChildren<UI_Fish>();
        ddr = GetComponentInChildren<DDR>();

    }

}
