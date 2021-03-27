using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionInfo : MonoBehaviour
{
    [TextArea]
    public string description;
    private PlayerInventory pInv;
    void Start()
    {
        pInv = FindObjectOfType<PlayerInventory>(true);
    }

    // Start is called before the first frame update
    void OnMouseEnter()
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
}
