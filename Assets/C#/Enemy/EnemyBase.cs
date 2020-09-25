using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D myRigid;
    [HideInInspector]
    public EnemyMovement myMovement;
    [HideInInspector]
    public EnemyStats myStats;
    [HideInInspector]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        myMovement = (EnemyMovement)GetComponent("EnemyMovement");
        myStats = (EnemyStats)GetComponent("EnemyStats");
        myRigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
