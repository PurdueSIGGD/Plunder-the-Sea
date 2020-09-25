using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishingMinigame : MonoBehaviour
{

    public GameObject[] arrowBases;

    private Canvas canvas;
    private UI_Fish fish;
    private List<GameObject> arrowTargets = new List<GameObject>();
    private float arrowSpeed = 1.0f / 90.0f;

    private void Start()
    {

        canvas = GetComponentInParent<Canvas>();
        fish = GetComponentInChildren<UI_Fish>();
        SendArrow(0);

    }

    private void FixedUpdate()
    {
        
        for (int i = 0; i < arrowTargets.Count; i++)
        {
            arrowTargets[i].transform.position -= new Vector3(0, canvas.pixelRect.height * arrowSpeed, 0);
            if (arrowTargets[i].transform.position.y <= 0)
            {
                Destroy(arrowTargets[i]);
                arrowTargets.RemoveAt(i);
                //Send random arrow
                SendArrow((int)(Random.value * arrowBases.Length));

            }
        }

    }

    private void SendArrow(int type)
    {

        //0-3 Left, Up, Down, Right
        if (type < 0 || type >= arrowBases.Length)
        {
            return;
        }

        Transform baseTrans = arrowBases[type].transform;
        GameObject target = Instantiate<GameObject>(arrowBases[type]);
        target.transform.SetParent(baseTrans.parent);
        target.transform.position = baseTrans.position;
        target.transform.localPosition = baseTrans.localPosition;
        target.transform.localPosition += new Vector3(0, canvas.pixelRect.height, 0);
        arrowTargets.Add(target);

    }

}
