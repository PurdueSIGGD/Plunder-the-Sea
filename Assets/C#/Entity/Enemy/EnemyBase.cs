using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyStats))]
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
    public PlayerBase player;
    [HideInInspector]
    public IEnumerator mover;
    // Start is called before the first frame update
    void Start()
    { 
        myMovement = (EnemyMovement)GetComponent("EnemyMovement");
        myStats = (EnemyStats)GetComponent("EnemyStats");
        myRigid = GetComponent<Rigidbody2D>();
        myCombat = (EnemyCombat)GetComponent("EnemyCombat");
        player = GameObject.FindObjectOfType<PlayerBase>();
        myMovement.moving = true;
    }

}
