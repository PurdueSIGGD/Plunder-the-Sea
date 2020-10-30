using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMovement : StateMovement
{
    // The distance to charge from. Make below zero for always-on charging.
    public float chargeDistance = 5.0f;

    // How fast the enemy moves while charging.
    public float chargeSpeed = 20.0f;

    // How much charge-up time (in seconds) there is before a charge.
    public float chargeUpTime = 1.0f;

    // How long the enemy will charge for (in seconds).
    public float chargeDuration = 1.0f;

    // The time after a charge before the charge can be used again (enemy will move normally if able to)
    public float chargeCooldown = 0.5f;

    // The state of the charging enemy. There are 4 options, which resemble different points in its movement.
    public enum ChargeState
    {
        isChargingUp,
        isCharging,
        isRecharging,
        ready
    }
    public ChargeState chargeState = ChargeState.ready;

    // The target angle.
    private Vector2 targetAngle;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            // Charging movement AI
            switch (chargeState)
            {
                case ChargeState.ready:
                    // Check if in charging range
                    if (chargeDistance < 0f || PlayerDistance() <= chargeDistance)
                    {
                        // Stop moving
                        myBase.myRigid.velocity = Vector2.zero;

                        // Find the angle to the player
                        targetAngle = (myBase.player.transform.position - this.transform.position).normalized;

                        // Initiate Charge
                        chargeState = ChargeState.isChargingUp;
                        SetTarget(chargeUpTime);
                    }
                    else
                    {
                        // No target in charging range
                        MoveTowards();
                    }
                    break;

                case ChargeState.isChargingUp:
                    // Stop moving
                    myBase.myRigid.velocity = Vector2.zero;

                    // Advance state if it finishes charging up
                    if (OnTarget())
                    {
                        chargeState = ChargeState.isCharging;
                        SetTarget(chargeDuration);
                    }
                    break;

                case ChargeState.isCharging:
                    // Move at charge speed
                    myBase.myRigid.velocity = targetAngle.normalized * chargeSpeed;

                    // Advance state if it finishes charging
                    if (OnTarget())
                    {
                        chargeState = ChargeState.isRecharging;
                        SetTarget(chargeCooldown);
                    }
                    break;

                case ChargeState.isRecharging:
                    // Move normally
                    MoveTowards();

                    // Advance state if it finishes recharging
                    if (OnTarget())
                    {
                        chargeState = ChargeState.ready;
                    }
                    break;
            }
        }
    }
}