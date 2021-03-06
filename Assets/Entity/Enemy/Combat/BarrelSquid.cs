using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSquid : StateCombat
{
    // Sprite references
    public SpriteRenderer sprite;
    public Animator anim;

    // Const values to make coding easier
    const int flanking = (int)FlankMovement.FlankState.flanking;
    const int stationary = (int)FlankMovement.FlankState.stationary;
    const int stopping = (int)FlankMovement.FlankState.stopping;
    const int starting = (int)FlankMovement.FlankState.starting;

    // Barrel Attribute modifier
    EntityAttribute barrelDefense = new EntityAttribute(ENT_ATTR.ARMOR_STATIC, 25);

    // How long the barrel squid has to wait before firing
    public float firingCooldown = 0.4f;
    private float firingTracker = 0;

    // Ink Shot projectile
    public GameObject inkShot;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        prevState = GetState();
    }

    // Update is called once per frame
    void Update()
    {
        // Variable to ensure that the state used for comparison doesn't change partway through Update()
        int current = GetState();

        anim.SetInteger("State", current);

        switch (current)
        {
            case flanking:
                if (prevState == starting)
                {
                    // Just started moving, so add defense
                    myBase.myStats.AddAttribute(barrelDefense, myBase.myStats);

                }

                // Animation
                //sprite.flipX = isPlayerLeft();
                //sprite.flipY = isPlayerUp();

                if (myBase != null && myBase.myRigid != null && myBase.myRigid.velocity.magnitude >= 0.001f)
                {
                    anim.speed = 1.0f;

                    // Rotate the sprite
                    Vector3 v = myBase.myRigid.velocity;
                    float angle = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg - 90;
                    sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                } else
                {
                    // Don't rotate while not moving
                    anim.speed = 0f;
                }
                

                break;
            case stopping:
                if (prevState == flanking)
                {
                    // Reset animation and rotation
                    sprite.transform.rotation = Quaternion.identity;
                    anim.speed = 1.0f;
                }
                break;
            case stationary:

                // Gameplay stuff
                if (prevState == stopping)
                {
                    // Just stopped moving, so remove defense
                    myBase.myStats.RemoveAttribute(barrelDefense);

                    firingTracker = SetTarget(firingCooldown);
                }
                // Try to shoot if possible and the cooldown allows
                if (OnTarget(firingTracker))
                {
                    Shoot(inkShot);
                    firingTracker = SetTarget(firingCooldown);
                } 
                break;
        }
        prevState = current;
    }
}
