﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fish : MonoBehaviour
{

    public float fishMovementSpeed = 4.0f;
    public float fishShakeSpeed = 6.0f;
    public float fishShakeAngle = 15.0f;

    private Vector2 fishCenter;
    private Vector2 fishMoveRadii = new Vector2(150, 50);

    void Start()
    {

        fishCenter = transform.localPosition;

    }

    void Update()
    {

        transform.localPosition = new Vector3(fishCenter.x + Mathf.Cos(Time.time * fishMovementSpeed) * fishMoveRadii.x, fishCenter.y + Mathf.Sin(Time.time * fishMovementSpeed) * fishMoveRadii.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, fishShakeAngle * Mathf.Sin(Time.time * fishShakeSpeed));

    }
}
