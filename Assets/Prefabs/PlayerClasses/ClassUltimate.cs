using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassUltimate : MonoBehaviour
{
    public SpriteRenderer aura;
    [HideInInspector]
    public int ultStacks = 0;
    [SerializeField]
    private GameObject PopupText;
    private PlayerStats pStats;
    private PlayerBase pBase;
    private int savedClass = -1;
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
                break;
            case 5:     //Swash Buckler
                break;
            case 6:     //Warrant Officer
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
                cost = 2;
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
                cost = 999;
                break;
            case 5:     //Swash Buckler
                cost = 999;
                break;
            case 6:     //Warrant Officer
                distributed = true;
                cost = 2;
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

    public void activate()
    {
        aura.gameObject.SetActive(true);
        ultStacks++;
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
                break;
            case 5:     //Swash Buckler
                break;
            case 6:     //Warrant Officer
                break;
        }
    }

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


}
