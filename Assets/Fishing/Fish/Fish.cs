using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;
    public float randomX;
    public float randomY;
    public float minWaitTime;
    public float maxWaitTime;
    private Vector3 randomPos;
    public float buffMovementSpeed = 0f;
    public float buffMaxStamina = 0f;
    public float buffMaxHP = 0f;
    public float buffStaminaRechargeRate = 0f;
    public int lootLevel;
    public GameObject FishingMinigame;
    private const float rotationSpeed = 0.1f;

    void Start()
    {
        RandomPosition();
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }

    public void BuffPlayerStats(PlayerStats stats)
    {
        stats.movementSpeed += buffMovementSpeed;
        stats.maxHP += buffMaxHP;
        stats.staminaMax += buffMaxStamina;
        stats.staminaRechargeRate += buffStaminaRechargeRate;
    }

    // finds random position for fish
    void RandomPosition()
    {
        randomPos = new Vector3(Random.Range(-randomX, randomX), Random.Range(-randomY, randomY), 0);
        StartCoroutine(MoveRandomly());
    }

    void IncrementRotation(Vector3 newRotation)
    {
        while (true) // This makes the target angle be within 180 degrees of where the fish is facing.
        {
            if(transform.eulerAngles.z + 180f < newRotation.z)
            {
                newRotation.z -= 360f;
            }
            else if(transform.eulerAngles.z - 180f > newRotation.z)
            {
                newRotation.z += 360f;
            }
            else
            {
                break;
            }
        }
        float z = newRotation.z;
        if(transform.eulerAngles.z > z)
        {
            if(transform.eulerAngles.z > z + (speed * rotationSpeed))
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - (speed * rotationSpeed));
            }
            else
            {
                transform.eulerAngles = newRotation;
            }
        }
        else
        {
            if(transform.eulerAngles.z < z - (speed * rotationSpeed))
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + (speed * rotationSpeed));
            }
            else
            {
                transform.eulerAngles = newRotation;
            }
        }
    }

    // moves the fish to the random location from the current position
    IEnumerator MoveRandomly(float angle = 0, bool useAngle = false)
    {
        float i = 0.0f;
        if(!useAngle)
        {
            angle = transform.eulerAngles.z + Random.Range(-90f, 90f);
        }

        // moves fish to random location
        while (i < 1.0f)
        {
            i += Time.deltaTime * (speed / 3);
            IncrementRotation(new Vector3(0, 0, angle));
            transform.position += transform.up * Time.deltaTime * speed;
            yield return null;
        }

        float randomFloat = Random.Range(0.0f, 1.0f);
        if (randomFloat < 0.5f)
            StartCoroutine(WaitBetweenMovements());
        else
            RandomPosition();
    }

    // fish waits at position if called then chooses another random location
    IEnumerator WaitBetweenMovements()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        RandomPosition();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if it hits a wall then stop and go the other direction
        if(col.gameObject.CompareTag("Pond"))
        {
            Vector3 dir = col.contacts[0].point;
            float angle = Mathf.Atan2(dir.y - transform.position.y, dir.x - transform.position.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
            StopAllCoroutines();
            StartCoroutine(MoveRandomly(-angle, true));
        }
    }
}
