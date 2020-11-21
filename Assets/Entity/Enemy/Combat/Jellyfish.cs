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
    public bool isSticking = false;
    public bool isCooldown = false;
    private Vector3 stickVector = Vector3.zero;
    private EntityStats stickVictim = null;

    private void Start()
    {
        myBase = GetComponent<EnemyBase>();
        myStateMovement = GetComponent<StateMovement>();
        stickSpeed = new EntityAttribute(ENT_ATTR.MOVESPEED, 0.5f, stickTime, true, false);
        stickDamage = new EntityAttribute(ENT_ATTR.POISON, 1f, stickTime, true);
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
        }

        prevState = current;
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
        isSticking = true;
    }

    void unstick(EntityStats target)
    {
        target?.RemoveAttribute(stickSpeed);
        target?.RemoveAttribute(stickDamage);
        cooldownTarget = SetTarget(cooldownTime);
        isCooldown = true;
        isSticking = false;
    }
}
