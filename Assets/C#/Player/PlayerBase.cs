using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D myRigid;
    [HideInInspector]
    public PlayerMovement myMovement;
    [HideInInspector]
    public PlayerStats myStats;
    public RangedCombat rangedCombat;

    private Camera cam;

    void Start()
    {
        myMovement = GetComponent<PlayerMovement>();
        myStats = GetComponent<PlayerStats>();
        myRigid = GetComponent<Rigidbody2D>();
        rangedCombat = GetComponent<RangedCombat>();

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<Camera>();

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {

            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            rangedCombat.ShootAt(mouseWorldPos);

        }
    }
}
