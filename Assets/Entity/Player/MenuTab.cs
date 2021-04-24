using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTab : MonoBehaviour, IPointerEnterHandler
{
    public string title;
    [TextArea]
    public string descr1;
    [TextArea]
    public string descr2;
    public bool isFirst = false;
    public bool classTab = false;
    private PlayerMenu menu;

    // Start is called before the first frame update
    void Start()
    {
        if (classTab)
        {
            classDescr();
        }
        menu = GetComponentInParent<PlayerMenu>();
        if (isFirst)
        {
            menu.setTab(this);
        }
    }

    public void classDescr()
    {
        PlayerClasses pclass = GetComponentInParent<PlayerClasses>();
        title = pclass.classes[pclass.classNumber].gameObject.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menu.setTab(this);
    }

}
