using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamCannon : StateCombat
{
    // Sprite references
    public SpriteRenderer sprite;
    public Animator anim;

    // Variables for operating
    private static LayerMask mask;
    private Vector3 playerAngle = Vector3.zero;
    private float searchTarget = 0f;
    private float shootTarget = 0f;

    // The timing on how long the search cooldown takes
    public float searchCooldown = 0f;
    public float shootCooldown = 1f;
    public float shootDistance = 8f;

    // Pearl Shot projectile
    public GameObject pearlShot;

    void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        if (searchCooldown <= 0f)
        {
            searchCooldown = myStateMovement.pathingRefresh;
        }
        prevState = GetState();
        mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (OnTarget(searchTarget))
        {
            //Update the player angle if the player isn't behind a wall
            if (myBase.player != null && !Physics2D.Linecast(transform.position, myBase.player.transform.position, mask))
            {
                playerAngle = myBase.player.transform.position - transform.position;
            }
            searchTarget = SetTarget(searchCooldown);
        }

        // Shoot if off cooldown and within distance
        if(OnTarget(shootTarget) && myStateMovement.PlayerDistance() < shootDistance && playerAngle != Vector3.zero)
        {
            anim.SetInteger("State", 1);
            sprite.flipX = isPlayerLeft();
            Projectile p = Shoot(pearlShot, transform.position, transform.position + playerAngle);
            shootTarget = SetTarget(shootCooldown);
        }

        if (myStateMovement.PlayerDistance() > shootDistance)
        {
            anim.SetInteger("State", 0);
        }
    }
}