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
    // Start is called before the first frame update
    void Start()
    {
        myMovement = (PlayerMovement)GetComponent<PlayerMovement>();
        myStats = (PlayerStats)GetComponent<PlayerStats>();
        myRigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
