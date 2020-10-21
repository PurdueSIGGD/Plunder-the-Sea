using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DDR : MonoBehaviour
{

    private Canvas canvas;
    [SerializeField]
    private GameObject[] arrowBases = null;
    [SerializeField]
    private Text scoreText = null;
    private List<GameObject>[] arrowTargets = new List<GameObject>[4];

    private float targetSpeed = 1.0f;//Seconds for target to travel entire height
    private int perfectScore = 20;//Score granted for "perfect" target hit
    private float perfectDistRatio = 0.25f;//Ratio of target size that counts as perfect
    private int currentScore = 0;
    private float targetFrequency = 0.5f;//Seconds for target to spawn
    private float nextTargetTime = 0.0f;//Time when to spawn new target

    private int catchScore = 500; //score needed to "catch" the fish
    private int releaseScore = -100; //score needed to "release" the fish

    public Fish fishBeingCaught;
    public PlayerStats targetPlayerStats;
    [SerializeField]
    private GameObject FishingMinigame;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        for (int i = 0; i < arrowTargets.Length; i++) { arrowTargets[i] = new List<GameObject>(); }
        nextTargetTime = Time.time + targetFrequency;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public float GetFrequency()
    {
        return targetFrequency;
    }

    public void SetFrequency(float sec)
    {
        nextTargetTime += (sec - targetFrequency);
        targetFrequency = sec;
    }

    public float GetTargetSpeed()
    {
        return targetSpeed;
    }

    public void SetTargetSpeed(float speed)
    {
        targetSpeed = speed;
    }

    public float GetCompletionPercentage()
    {
        if (currentScore >= 0) {
            return (float)currentScore / (float)catchScore;
        }
        else
        {
            return (float)-currentScore / (float)releaseScore;
        }
    }

    void Update()
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

                }

                currentScore += score;


            }

            //Move targets
            for (int j = 0; j < arrowTargets[i].Count; j++)
            {
                GameObject target = arrowTargets[i][j];
                target.transform.position -= new Vector3(0, canvas.pixelRect.height * targetSpeed * Time.deltaTime, 0);
                if (target.transform.position.y <= 0)
                {
                    //Target hit bottom, complete miss
                    currentScore -= perfectScore;
                    Destroy(target);
                    arrowTargets[i].RemoveAt(j);

                }
            }
        }

        //Update score text
        scoreText.text = "Score: " + currentScore;

        //logic for ending the scene
        if (currentScore >= catchScore)
        {
            fishBeingCaught.BuffPlayerStats(targetPlayerStats);
            ResetMinigame();
        }
        else if (currentScore <= releaseScore)
        {
            ResetMinigame();
        }

        //Send random target
        if (Time.time >= nextTargetTime)
        {
            SendArrow((int)(Random.value * arrowBases.Length));
            nextTargetTime = Time.time + targetFrequency;
        }

    }

    private void ResetMinigame()
    {
        currentScore = 0;
        FishingMinigame.SetActive(false);
        targetPlayerStats.GetComponent<PlayerMovement>().enabled = true;
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
