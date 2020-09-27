using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FishingMinigame : MonoBehaviour
{

    public GameObject[] arrowBases;
    public Text scoreText;

    private Canvas canvas;
    private UI_Fish fish;
    private List<GameObject>[] arrowTargets = new List<GameObject>[4];
    private float arrowSpeed = 1.0f / 1000.0f;//Distance per frame based on canvas height
    private int perfectScore = 20;//Score granted for "perfect" target hit
    private float perfectDistRatio = 0.25f;//Ratio of target size that counts as perfect
    private int currentScore = 0;

    private void Start()
    {

        canvas = GetComponentInParent<Canvas>();
        fish = GetComponentInChildren<UI_Fish>();
        for (int i = 0; i < arrowTargets.Length; i++) { arrowTargets[i] = new List<GameObject>(); }

        SendArrow(0);

    }

    private void Update()
    {

        bool[] input = { Input.GetKeyDown(KeyCode.LeftArrow), Input.GetKeyDown(KeyCode.UpArrow), Input.GetKeyDown(KeyCode.DownArrow), Input.GetKeyDown(KeyCode.RightArrow) };
        
        for (int i = 0; i < arrowTargets.Length; i++)
        {
            //Input check
            if (input[i])
            {

                int score = -perfectScore;//Complete miss is negative perfect

                //Check for single target hit
                if (arrowTargets[i].Count > 0)
                {

                    GameObject target = arrowTargets[i][0];
                    //Absolute distance of target from base
                    float dist = Mathf.Abs(((Vector2)target.gameObject.transform.position - (Vector2)arrowBases[i].transform.position).magnitude);
                    //Make distance in units of target size
                    dist /= target.GetComponent<RectTransform>().rect.height;
                    if (dist <= perfectDistRatio)
                    {
                        score = perfectScore;
                    }
                    else if (dist <= perfectDistRatio * 2)
                    {
                        score = perfectScore / 2;
                    }
                    else if (dist <= perfectDistRatio * 4)
                    {
                        score = perfectScore / 4;
                    }

                    Destroy(target);
                    arrowTargets[i].RemoveAt(0);
                    //Send random target
                    SendArrow((int)(Random.value * arrowBases.Length));

                }

                currentScore += score;

            }

            //Move targets
            for (int j = 0; j < arrowTargets[i].Count; j++)
            {
                GameObject target = arrowTargets[i][j];
                target.transform.position -= new Vector3(0, canvas.pixelRect.height * arrowSpeed, 0);
                if (target.transform.position.y <= 0)
                {
                    //Target hit bottom, complete miss
                    currentScore -= perfectScore;

                    Destroy(target);
                    arrowTargets[i].RemoveAt(j);
                    //Send random target
                    SendArrow((int)(Random.value * arrowBases.Length));

                }
            }
        }

        //Update score text
        scoreText.text = "Score: " + currentScore;

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
        arrowTargets[type].Add(target);

    }

}
