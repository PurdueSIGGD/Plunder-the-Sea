using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassUltimate : MonoBehaviour
{
    public SpriteRenderer aura;
    public GameObject smokeBomb;
    [HideInInspector]
    public int ultStacks = 0;
    [SerializeField]
    private GameObject PopupText;
    private PlayerStats pStats;
    private PlayerBase pBase;
    private int savedClass = -1;
    private float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        pStats = GetComponent<PlayerStats>();
        pBase = GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    public bool callUlt(int classIndex)
    {
        savedClass = classIndex;
        int consumed = consumeBait(classIndex);
        if (consumed <= 0)
        {
            return false;
        }
        switch (classIndex)
        {
            case 0:     //Test
                break;
            case 1:     //Brawler
                brawlerUlt();
                break;
            case 2:     //Gunner
                gunnerUlt(consumed);
                break;
            case 3:     //Captain
                captainUlt();
                break;
            case 4:     //First Mate
                mateUlt();
                break;
            case 5:     //Swash Buckler
                swashUlt();
                break;
            case 6:     //Warrant Officer
                warrantUlt();
                break;
        }
        return true;
    }

    public int consumeBait(int classIndex)
    {
        savedClass = classIndex;
        bool partial = false;        //Can ult be performed on partial bait cost
        bool distributed = false;    //Is cost on currently selected bait, or on all baits simultaneously
        int cost = 999;                //for distributed, cost on each bait; for not, total cost
        switch (classIndex)
        {
            case 0:     //Test
                cost = 999;
                break;
            case 1:     //Brawler
                if (ultStacks >= 5)
                {
                    return -1;
                }
                distributed = true;
                cost = 1;
                break;
            case 2:     //Gunner
                if (pStats.ammo >= pStats.maxAmmo)
                {
                    return -1;
                }
                partial = true;
                cost = pStats.maxAmmo;
                break;
            case 3:     //Captain
                if (pStats.ContainsAttribute("pride"))
                {
                    return -1;
                }
                cost = 5;
                break;
            case 4:     //First Mate
                if (ultStacks >= 1)
                {
                    return -1;
                }
                cost = 5;
                break;
            case 5:     //Swash Buckler
                cost = 5;
                break;
            case 6:     //Warrant Officer
                if (ultStacks >= 1)
                {
                    return -1;
                }
                distributed = true;
                cost = 1;
                break;
            default:
                return -1;
        }
        int[] baits = pStats.baitInventory.baitTypes;
        if (!partial)
        {
            if (distributed)
            {
                for (int i = 0; i < baits.Length; i++)
                {
                    if (baits[i] < cost)
                    {
                        return -1;
                    }
                }
            }
            else
            {
                int sum = 0;
                for (int i = 0; i < baits.Length; i++)
                {
                    sum += baits[i];
                }
                if (sum < cost)
                {
                    return -1;
                }
            }   
        }
        int consumed = 0;
        if (distributed)
        {
            for (int i = 0; i < baits.Length; i++)
            {
                int amount = Mathf.Min(baits[i], cost);
                consumed += amount;
                pStats.baitInventory.removeBait(i, amount);
            }
        }
        else
        {
            consumed = cost;
            int start = pBase.fishing.getSelectedBait();
            for (int i = 0; i < baits.Length; i++)
            {
                int index = (start + i) % baits.Length;
                int amount = Mathf.Min(baits[index], consumed);
                pStats.baitInventory.removeBait(index, amount);
                consumed -= amount;
                if (consumed <= 0)
                {
                    break;
                }
            }
            consumed = cost - consumed;
        }

        return consumed;
    }

    // Gets called on adding the ULTSTATUS attribute
    public void activate()
    {
        aura.gameObject.SetActive(true);
        switch (savedClass)
        {
            case 0:     //Test
                break;
            case 1:     //Brawler
                pStats.weaponInv.weaponMods.meleeSizeMultiplier *= 1.5f;
                break;
            case 2:     //Gunner
                break;
            case 3:     //Captain
                break;
            case 4:     //First Mate
                pStats.lockAction();
                cooldown = 0;
                break;
            case 5:     //Swash Buckler
                break;
            case 6:     //Warrant Officer
                cooldown = 0;
                break;
        }

        ultStacks++;
    }

    // Only does something if at least one ULTSTATUS in effect
    // and that specific class performs specialized actions during effect
    void Update()
    {
        if (ultStacks <= 0)
        {
            return;
        }
        switch (savedClass)
        {
            case 0:     //Test
                break;
            case 1:     //Brawler
                break;
            case 2:     //Gunner
                break;
            case 3:     //Captain
                break;
            case 4:     //First Mate
                cooldown -= Time.deltaTime;
                float speed = pStats.movementSpeed;
                if (cooldown <= 0)
                {
                    cooldown += (.5f * 10 / speed);
                    pStats.weaponInv.ShootAt(pBase.getCamMousePos(), true, false);
                }
                
                Vector2 newVelocity = (pBase.getCamMousePos() - (Vector2)transform.position).normalized * speed * 1.5f * Time.deltaTime;

                pBase.rigidBody.AddForce(newVelocity * 64);
                break;
            case 5:     //Swash Buckler
                break;
            case 6:     //Warrant Officer
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    cooldown += .25f;
                    float distance = 8;
                    EnemyStats strike = null;
                    foreach (EnemyStats es in FindObjectsOfType<EnemyStats>())
                    {
                        float distPlaceholder = Vector2.Distance(transform.position, es.transform.position);
                        if (distPlaceholder < distance)
                        {
                            strike = es;
                            distance = distPlaceholder;
                        }
                    }
                    if (strike != null)
                    {
                        PlayerClasses pc = GetComponent<PlayerClasses>();
                        int saveRadius = pc.chainRadius;
                        int saveLength = pc.chainLength;
                        float saveChance = pc.chainChance;
                        bool saveSelf = pc.selfChain;
                        pc.chainRadius = 8;
                        pc.chainLength = 8;
                        pc.chainChance = 1;
                        pc.selfChain = true;
                        GameObject g = Instantiate(pc.lightingPrefab, transform.position, Quaternion.identity);
                        LineRenderer lr = g.GetComponent<LineRenderer>();
                        lr.SetPositions(new Vector3[] { transform.position, strike.transform.position });
                        lr.material.SetTextureScale("_MainTex", new Vector2(Vector3.Distance(lr.GetPosition(0), lr.GetPosition(1) / lr.widthMultiplier), 1));
                        Destroy(g, 0.5f);
                        pc.enemyHit(strike);
                        pc.chainRadius = saveRadius;
                        pc.chainLength = saveLength;
                        pc.chainChance = saveChance;
                        pc.selfChain = saveSelf;
                    }
                }

                break;
        }
    }

    // gets called on removing the ULTSTATUS attribute
    public void deactivate()
    {
        if (!pStats.ContainsAttribute(ENT_ATTR.ULTSTATUS))
        {
            aura.gameObject.SetActive(false);
        }
        ultStacks--;
        switch (savedClass)
        {
            case 0:     //Test
                break;
            case 1:     //Brawler
                pStats.weaponInv.weaponMods.meleeSizeMultiplier = pStats.weaponInv.weaponMods.meleeSizeMultiplier / 1.5f;
                break;
            case 2:     //Gunner
                break;
            case 3:     //Captain
                break;
            case 4:     //First Mate
                pStats.unlockAction();
                break;
            case 5:     //Swash Buckler
                break;
            case 6:     //Warrant Officer
                break;
        }
    }

    public void SpawnPopupText(string text)
    {
        GameObject textObject = Instantiate(PopupText, transform.position, Quaternion.identity);
        textObject.transform.position = new Vector3(textObject.transform.position.x, textObject.transform.position.y + 2f, -2);
        TextMesh textMesh = textObject.GetComponent<TextMesh>();
        textMesh.text = text;
    }

    public void brawlerUlt()
    {
        EntityAttribute act = new EntityAttribute(ENT_ATTR.ULTSTATUS, 1, 3, false, true);
        pStats.AddAttribute(act, pStats);
        SpawnPopupText("Maximal\nExpansion");
    }

    public void gunnerUlt(int amount)
    {
        pStats.replenishAmmo(amount);
        EntityAttribute act = new EntityAttribute(ENT_ATTR.ULTSTATUS, 1, .2f, false, true);
        pStats.AddAttribute(act, pStats);
        SpawnPopupText("Sacrificial\nReload");
    }

    public void captainUlt()
    {
        EntityAttribute capPride = new EntityAttribute(ENT_ATTR.INVULNERABLE, 1, 5,false, true, "pride");
        EntityAttribute act = new EntityAttribute(ENT_ATTR.ULTSTATUS, 1, 5, false, true);
        pStats.AddAttribute(capPride, pStats);
        pStats.AddAttribute(act, pStats);
        SpawnPopupText("Captain's\nPride");
    }

    public void mateUlt()
    {
        EntityAttribute act = new EntityAttribute(ENT_ATTR.ULTSTATUS, 1, 2, false, true);
        pStats.AddAttribute(act, pStats);
        SpawnPopupText("Berserker's\nCharge");
    }

    public void swashUlt()
    {
        EntityAttribute act = new EntityAttribute(ENT_ATTR.ULTSTATUS, 1, .2f, false, true);
        pStats.AddAttribute(act, pStats);

        pStats.lockAction();
        //pStats.weaponInv.ShootAt(pBase.getCamMousePos(), false, false);
        int mask = LayerMask.GetMask("Wall");
        float maxDist = 3;
        Vector2 playerPos = (Vector2)transform.position;
        Vector2 dir = pBase.getCamMousePos() - playerPos;
        GameObject bomb = Instantiate(smokeBomb, transform.position, Quaternion.identity);
        bomb.GetComponent<Projectile>().SetSource(pBase.gameObject);
        RaycastHit2D hit = Physics2D.Raycast(playerPos, dir, maxDist, mask);
        Vector2 tp = playerPos + maxDist * dir.normalized;
        if (hit.collider != null)
        {
            tp = playerPos + hit.distance * dir.normalized;
        }
        transform.position = tp;
        pStats.weaponInv.ShootAt(pBase.getCamMousePos(), true, false);
        pStats.weaponInv.ShootAt(2*playerPos - pBase.getCamMousePos(), false, false);
        pStats.unlockAction();


        SpawnPopupText("Nothin\nPersonal");
    }

    public void warrantUlt()
    {
        EntityAttribute act = new EntityAttribute(ENT_ATTR.ULTSTATUS, 1, 5, false, true);
        pStats.AddAttribute(act, pStats);
        /*
        PlayerClasses pc = GetComponent<PlayerClasses>();
        int saveRadius = pc.chainRadius;
        int saveLength = pc.chainLength;
        float saveChance = pc.chainChance;
        pStats.lockAction();
        pc.chainRadius = 8;
        pc.chainLength = 8;
        pc.chainChance = 1;
        foreach (EnemyStats es in FindObjectsOfType<EnemyStats>())
        {
            float distance = Vector2.Distance(transform.position, es.transform.position);
            if (distance <= 8)
            {
                pc.enemyHit(es);
            }
        }

        pc.chainRadius = saveRadius;
        pc.chainLength = saveLength;
        pc.chainChance = saveChance;
        pStats.unlockAction();
        */
        SpawnPopupText("Lightning\nLord");
    }

}
