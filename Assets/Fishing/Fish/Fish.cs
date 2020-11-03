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
    public float buffMovementSpeed = 0f;
    public float buffMaxStamina = 0f;
    public float buffMaxHP = 0f;
    public float buffStaminaRechargeRate = 0f;
    public int lootLevel;
    public int preferredBaitType;
    public GameObject FishingMinigame;
    private const float rotationSpeed = 0.25f;

    void Start()
    {
        StartCoroutine(MoveRandomly());
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }

    public void BuffPlayerStats(PlayerStats stats)
    {
        stats.movementSpeed += buffMovementSpeed;
        stats.maxHP += buffMaxHP;
        stats.staminaMax += buffMaxStamina;
        stats.staminaRechargeRate += buffStaminaRechargeRate;
    }

    bool PassedTarget(Vector3 oldPosition, Vector3 targetPos)
    {
        if((oldPosition.x < targetPos.x && transform.position.x > targetPos.x) ||
            (oldPosition.y < targetPos.y && transform.position.y > targetPos.y) ||
            (oldPosition.x > targetPos.x && transform.position.x < targetPos.x) ||
            (oldPosition.y > targetPos.y && transform.position.y < targetPos.y))
        {
            return true;
        }
        else
        {
            return false;
        }
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
            transform.position += transform.right * Time.deltaTime * speed;
            yield return null;
        }

        float randomFloat = Random.Range(0.0f, 1.0f);
        if (randomFloat < 0.5f)
            StartCoroutine(WaitBetweenMovements());
        else
            StartCoroutine(MoveRandomly());
    }

    IEnumerator MoveToSpecificPos(Vector3 pos)
    {
        float i = 0.0f;
        float angle = Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x) * Mathf.Rad2Deg;
        Vector3 oldPosition;

        // moves fish to targeted location
        while (i < 1.0f)
        {
            i += Time.deltaTime * speed;
            IncrementRotation(new Vector3(0, 0, angle));
            oldPosition = transform.position;
            transform.position += transform.right * Time.deltaTime * speed;
            if(PassedTarget(oldPosition, pos))
            {
                Debug.Log("Reached bobber.");
                StartCoroutine(WaitForever());
            }
            yield return null;
        }

        float randomFloat = Random.Range(0.0f, 1.0f);
        if (randomFloat < 0.5f)
            StartCoroutine(WaitBetweenMovements());
        else
            StartCoroutine(MoveRandomly());
    }

    // fish has been "caught" and now waits
    IEnumerator WaitForever()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        StartCoroutine(WaitForever());
    }

    // fish waits at position if called then chooses another random location
    IEnumerator WaitBetweenMovements()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        StartCoroutine(MoveRandomly());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if it hits a wall then stop and go the other direction
        if(col.gameObject.CompareTag("Pond"))
        {
            Vector3 dir = col.contacts[0].point;
            float angle = (Mathf.Atan2(dir.y - transform.position.y, dir.x - transform.position.x) * Mathf.Rad2Deg) + 180f;
            StopAllCoroutines();
            StartCoroutine(MoveRandomly(2 * angle - (180f + transform.eulerAngles.z) % 360f, true));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bobber") && !col.gameObject.GetComponent<Bobber>().casting)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToSpecificPos(col.gameObject.transform.position));
        }
    }
}
