using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordMove : MonoBehaviour
{

    private RectTransform RT;
    private Image image;
    private float rotMultiplier;
    private float linMultiplier;
    private float alpha = 2;

    private void Start()
    {
        RT = GetComponent<RectTransform>();
        linMultiplier = Random.Range(1f, 4f);
        rotMultiplier = (Random.Range(0, 2) * 2 - 1) * linMultiplier;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        RT.localPosition += Vector3.up * Time.deltaTime * 64 * linMultiplier;
        RT.localEulerAngles = new Vector3(0, RT.localEulerAngles.y + Time.deltaTime * 90 * rotMultiplier, 0);
        alpha -= Time.deltaTime * linMultiplier / 4;
        image.color = new Color(1, 1, 1, alpha);
    }
}
