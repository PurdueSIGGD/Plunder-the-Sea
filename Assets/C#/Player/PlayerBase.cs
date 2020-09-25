using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStats))]
[RequireComponent(typeof(RangedCombat))]
public class PlayerBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D myRigid;
    [HideInInspector]
    public PlayerMovement myMovement;
    [HideInInspector]
    public EntityStats myStats;
    [HideInInspector]
    public RangedCombat rangedCombat;

    private UI_Camera cam;

    void Start()
    {
        myMovement = GetComponent<PlayerMovement>();
        myStats = GetComponent<EntityStats>();
        myRigid = GetComponent<Rigidbody2D>();
        rangedCombat = GetComponent<RangedCombat>();

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<UI_Camera>();

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {

            rangedCombat.ShootAt(cam.GetMousePosition());

        }
    }
}
