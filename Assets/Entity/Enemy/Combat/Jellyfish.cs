using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : StateCombat
{
    // How long the jellyfish should stick for
    public float stickTime = 2.5f;
    public float cooldownTime = 1.0f;

    // The attributes describing what happens when the jellyfish sticks to the player
    public EntityAttribute stickSpeed;
    public EntityAttribute stickDamage;

    // Const values to make coding easier
    const int charging = (int)ChargeMovement.ChargeState.isCharging;

    // The current state
    private int current = 0;

    // The target timing for sticking onto the player
    private float stickTarget = 0.0f;
    private float cooldownTarget = 0.0f;
    private bool isSticking = false;
    private bool isCooldown = false;
    private Vector3 stickVector = Vector3.zero;
    private EntityStats stickVictim = null;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        stickSpeed = new EntityAttribute(ENT_ATTR.MOVESPEED, 0.5f, stickTime, true, false);
        stickDamage = new EntityAttribute(ENT_ATTR.POISON, 1f, stickTime, true);
        anim.SetInteger("Variant", Random.Range(0, 2));
        prevState = GetState();
    }

    // Update is called once per frame
    void Update()
    {
        current = GetState();

        if (isSticking && OnTarget(stickTarget))
        {
            unstick(stickVictim);
        }
        if (isCooldown && OnTarget(cooldownTarget))
        {
            myBase.myMovement.moving = true;
            isCooldown = false;
        }
        if (isSticking)
        {
            transform.position = stickVictim.transform.position + stickVector;
            myBase.myRigid.velocity = Vector3.zero;
            
        } else
        {
            anim.SetInteger("State", 0);
        }

        prevState = current;
    }

    // Called right before the enemy dies.
    public override void OnDeath()
    {
        if (isSticking)
        {
            unstick(stickVictim);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) //Called when something enters the enemy's range, only activates if charging
    {
        if (current == charging && collider.GetComponent<PlayerBase>())
        {
            if (!isCooldown && !isSticking)
            {
                myBase.myRigid.velocity = Vector2.zero;
                stick(collider.GetComponent<PlayerStats>());
                stickTarget = SetTarget(stickTime);
            }
        }
    }
    void stick(EntityStats target) //Sticks onto the target
    {
        myBase.myMovement.moving = false;
        target.AddAttribute(stickSpeed,myBase.myStats);
        target.AddAttribute(stickDamage, myBase.myStats);
        stickVictim = target;
        stickVector = transform.position - target.transform.position;
        float angle = Mathf.Atan2(stickVector.y, stickVector.x) * Mathf.Rad2Deg - 90;
        sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        anim.SetInteger("State", 1);
        isSticking = true;
    }

    void unstick(EntityStats target)
    {
        target?.RemoveAttribute(stickSpeed);
        target?.RemoveAttribute(stickDamage);
        cooldownTarget = SetTarget(cooldownTime);
        sprite.transform.rotation = Quaternion.identity;
        anim.SetInteger("State", 0);
        isCooldown = true;
        isSticking = false;
    }
}
