using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionInfo : MonoBehaviour, IPointerEnterHandler
{
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
        pInv.Description.text = description;
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
