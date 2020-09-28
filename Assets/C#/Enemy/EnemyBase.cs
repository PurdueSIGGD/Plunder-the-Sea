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
    public EnemyCombat myCombat;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public IEnumerator mover;
    // Start is called before the first frame update
    void Start()
    { 
        myMovement = (EnemyMovement)GetComponent("EnemyMovement");
        myStats = (EnemyStats)GetComponent("EnemyStats");
        myRigid = GetComponent<Rigidbody2D>();
        myCombat = (EnemyCombat)GetComponent("EnemyCombat");
        player = GameObject.FindWithTag("Player");
        mover = myMovement.move();
        StartCoroutine(mover);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StopCoroutine(mover);
        }
    }
}
