using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditRandomizer : MonoBehaviour
{
    private string[] names = {"Tom Yarrow", "Andrew Choi", "Phoebus Yang", "Andy Sharpe", "Michael Forte", "Clayton Detke", "Landon Ellis",
        "Alex Plump", "Michael Beshear", "Sam Schafer", "Chase Fretch", "Nicole Tomaszewski", "Sean Thomas", "Paolo Ayos"};
    private List<string> nameList = new List<string>();
    [SerializeField]
    private Text[] nameSlots;

    private void Start()
    {
        foreach(string s in names)
        {
            nameList.Add(s);
        }
        foreach (Text t in nameSlots)
        {
            if (nameList.Count > 0)
            {
                string chosen = nameList[Random.Range(0, nameList.Count)];
                t.text = chosen;
                nameList.Remove(chosen);
            }
        }
    }
}
