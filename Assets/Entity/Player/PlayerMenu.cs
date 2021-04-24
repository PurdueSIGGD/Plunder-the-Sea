using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public Text title;
    public Text descr1;
    public Text descr2;

    public void setTab(MenuTab tab)
    {
        title.text = tab.title;
        descr1.text = tab.descr1;
        descr2.text = tab.descr2;
    }
}
