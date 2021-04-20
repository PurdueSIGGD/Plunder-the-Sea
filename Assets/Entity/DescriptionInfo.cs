using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionInfo : MonoBehaviour, IPointerEnterHandler
{

    [SerializeField]
    private string name;

    [SerializeField]
    private string buff;

    [TextArea]
    public string description;
    private PlayerInventory pInv;
    void Start()
    {
        pInv = FindObjectOfType<PlayerInventory>(true);
    }

    public void updateInventoryDescription()
    {
        if (pInv == null)
        {
            pInv = FindObjectOfType<PlayerInventory>(true);
        }
        if (pInv == null)
        {
            return;
        }
        pInv.fishName.text = name;
        pInv.fishImage.sprite = GetComponent<SpriteRenderer>().sprite;
        pInv.fishBait.sprite = pInv.baitImages[GetComponent<Fish>().preferredBaitType];
        pInv.fishDescription.text = description;
        pInv.invCounter = 12;
        pInv.fishBuffName.text = buff;
        pInv.fishFrame.SetActive(true);
    }

    // Start is called before the first frame update
    void OnMouseEnter()
    {
        updateInventoryDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        updateInventoryDescription();
    }
}
