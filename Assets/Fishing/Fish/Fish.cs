using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed = 0.35f;
    public int preferredBaitType;
    public float wrongBaitCatchPercent = 0.25f;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;
    public int lootLevel;
    public float[] buffs = {0f, 0f, 0f, 0f, 0f, 0f, 0f};
    public float[] buffsMult = {0f, 0f, 0f, 0f, 0f, 0f, 0f};
    private float[] buffsApplied;
    public string[] buffNames = { "Movement Speed", "HP", "Stamina", "Stamina Recharge Rate", "Melee Damage",
        "Ranged Damage", "Armor", };
    public GameObject FishingMinigame;

    public Sprite sprite;

    void Start()
    {
        StartCoroutine(MoveRandomly());
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        sprite = this.GetComponent<SpriteRenderer>().sprite;
        buffsApplied = new float[buffsMult.Length];
    }

    public void BuffPlayerStats(PlayerBase player)
    {
        player.stats.movementSpeed += buffs[0];
        player.stats.maxHP += buffs[1];
        player.stats.staminaMax += buffs[2];
        player.stats.staminaRechargeRate += buffs[3];
        player.stats.weaponInv.weaponMods.meleeDamageAddition += buffs[4];
        player.stats.weaponInv.weaponMods.rangedDamageAddition += buffs[5];
        player.stats.armorStatic += buffs[6];

        buffsApplied[0] = player.stats.movementSpeed * buffsMult[0];
        buffsApplied[1] = player.stats.maxHP * buffsMult[1];
        buffsApplied[2] = player.stats.staminaMax * buffsMult[2];
        buffsApplied[3] = player.stats.staminaRechargeRate * buffsMult[3];
        buffsApplied[4] = buffsMult[4];
        buffsApplied[5] = buffsMult[5];
        buffsApplied[6] = (1 - player.stats.armorMult) * buffsMult[6];

        player.stats.movementSpeed += buffsApplied[0];
        player.stats.maxHP += buffsApplied[1];
        player.stats.staminaMax += buffsApplied[2];
        player.stats.staminaRechargeRate += buffsApplied[3];
        player.stats.weaponInv.weaponMods.meleeDamageMultiplier += buffsApplied[4];
        player.stats.weaponInv.weaponMods.rangedDamageMultiplier += buffsApplied[5];
        player.stats.armorMult += buffsApplied[6];

        string text = "";
        for(int i = 0; i < buffs.Length; i++)
        {
            if(buffs[i] > 0f)
            {
                text += "+" + buffs[i] + " " + buffNames[i] + "\n";
            }
            if (buffsApplied[i] > 0f)
            {
                text += "+" + buffsApplied[i] + " " + buffNames[i] + "\n";
            }
        }
        player.fishing.SpawnPopupText(text);
    }

    public void UnbuffPlayerStats(PlayerBase player)
    {
        player.stats.movementSpeed -= buffsApplied[0];
        player.stats.maxHP -= buffsApplied[1];
        player.stats.staminaMax -= buffsApplied[2];
        player.stats.staminaRechargeRate -= buffsApplied[3];
        player.stats.weaponInv.weaponMods.meleeDamageMultiplier -= buffsApplied[4];
        player.stats.weaponInv.weaponMods.rangedDamageMultiplier -= buffsApplied[5];
        player.stats.armorMult -= buffsApplied[6];

        string text = "";
        for (int i = 0; i < buffs.Length; i++)
        {
            if (buffsApplied[i] > 0f)
            {
                text += "-" + buffsApplied[i] + " " + buffNames[i] + "\n";
            }
        }
        player.fishing.SpawnPopupText(text);
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
            i += Time.deltaTime * (speed / 2);
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
                StartCoroutine(WaitForALittle());
                yield break;
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
    IEnumerator WaitForALittle()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(MoveRandomly());
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
        if (col.gameObject.CompareTag("Bobber") && !col.gameObject.GetComponent<Bobber>().casting)
        {
            StopAllCoroutines();
            StartCoroutine(WaitForALittle());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bobber"))
        {
            Bobber bobber = col.gameObject.GetComponent<Bobber>();
            if (!bobber.casting && bobber.baitType == preferredBaitType)
            {
                StopAllCoroutines();
                StartCoroutine(MoveToSpecificPos(col.gameObject.transform.position));
            }
        }
    }
}
