using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStats))]
[RequireComponent(typeof(RangedCombat))]
[RequireComponent(typeof(MeleeCombat))]
public class PlayerBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public EntityStats stats;
    [HideInInspector]
    public RangedCombat rangedCombat;
    [HideInInspector]
    public MeleeCombat meleeCombat;

    private UI_Camera cam;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<EntityStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        rangedCombat = GetComponent<RangedCombat>();
        meleeCombat = GetComponent<MeleeCombat>();

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<UI_Camera>();

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            rangedCombat.ShootAt(cam.GetMousePosition());
        }

        if (Input.GetKey(KeyCode.E))
        {
            meleeCombat.ShootAt(cam.GetMousePosition());
        }
    }
}
