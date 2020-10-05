using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(RangedCombat))]
[RequireComponent(typeof(MeleeCombat))]
[RequireComponent(typeof(PlayerFishing))]
public class PlayerBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public RangedCombat rangedCombat;
    [HideInInspector]
    public MeleeCombat meleeCombat;
    [HideInInspector]
    public PlayerFishing fishing;

    private UI_Camera cam;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        rangedCombat = GetComponent<RangedCombat>();
        meleeCombat = GetComponent<MeleeCombat>();
        fishing = GetComponent<PlayerFishing>();

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<UI_Camera>();

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            rangedCombat.ShootAt(cam.GetMousePosition());
        }

        if (Input.GetButtonDown(1))
        {
            meleeCombat.ShootAt(cam.GetMousePosition());
        }
    }

    //PlayerStats calls this when player kills entity
    public void OnKill (EntityStats victim)
    {

        rangedCombat.RefreshCooldown();

    }

}
