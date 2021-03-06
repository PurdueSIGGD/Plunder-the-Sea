﻿using System.Collections;
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
    public List<GameObject>[] arrowTargets = new List<GameObject>[4];
    [SerializeField]
    private Sprite[] qualityImages;
    [SerializeField]
    private GameObject textPrefab;

    private float targetSpeed = 1.0f;//Seconds for target to travel entire height
    private int perfectScore = 20;//Score granted for "perfect" target hit
    private float perfectDistRatio = 0.25f;//Ratio of target size that counts as perfect
    private int currentScore = 0;
    private float targetFrequencyStep = 0.25f; // # of second per beat
    private int targetFrequencyMax = 4; // Max # of beats for target frequency
    private int targetFrequencyMin = 2; // Min # of beats for target frequency
    private float nextTargetTime = 0.0f;//Time when to spawn new target

    private int catchScore = 400; //score needed to "catch" the fish
    private int releaseScore = -100; //score needed to "release" the fish

    [SerializeField]
    private AudioClip[] songs;
    [SerializeField]
    private int[] bpm;
    private AudioSource AS;
    [SerializeField]
    private int[] maxStep;
    private int songNum = -1;

    public Fish fishBeingCaught;
    public PlayerBase targetPlayer;
    [SerializeField]
    private GameObject FishingMinigame = null;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        for (int i = 0; i < arrowTargets.Length; i++) { arrowTargets[i] = new List<GameObject>(); }
        nextTargetTime = Time.time + targetFrequencyMax*targetFrequencyStep - targetSpeed;
        AS = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioSetup();
    }

    public int GetScore()
    {
        return currentScore;
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
        if (Input.GetButtonDown("Cast Fishing Pole")) // F Key
        {
            ResetMinigame();
        }


        bool[] input = { Input.GetKeyDown(KeyCode.LeftArrow), Input.GetKeyDown(KeyCode.UpArrow), Input.GetKeyDown(KeyCode.DownArrow), Input.GetKeyDown(KeyCode.RightArrow),
                            Input.GetKeyDown(KeyCode.A), Input.GetKeyDown(KeyCode.W), Input.GetKeyDown(KeyCode.S), Input.GetKeyDown(KeyCode.D)};

        for (int i = 0; i < arrowTargets.Length; i++)
        {
            //Input check
            if (input[i] || input[i+4])
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

                    createWord((int) (dist/perfectDistRatio));

                    Destroy(target);
                    arrowTargets[i].RemoveAt(0);

                }

                currentScore += score;


            }

            //Move targets
            for (int j = 0; j < arrowTargets[i].Count; j++)
            {
                GameObject target = arrowTargets[i][j];
                target.transform.position -= new Vector3(0, canvas.pixelRect.height * Time.deltaTime / targetSpeed, 0);
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
            PlayerPrefs.SetInt("Caught", PlayerPrefs.GetInt("Caught") + 1);
            fishBeingCaught.BuffPlayerStats(targetPlayer);
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
            nextTargetTime = Time.time + Random.Range(targetFrequencyMin, targetFrequencyMax + 1) * targetFrequencyStep;
        }

    }

    private void ResetMinigame()
    {
        currentScore = 0;
        FishingMinigame.SetActive(false);
        targetPlayer.movement.enabled = true;
        targetPlayer.fishing.gameActive = false;
        
        for (int i = 0; i < arrowTargets.Length; i++) {
            foreach (GameObject arrow in arrowTargets[i])
            {
                Destroy(arrow);
            }
            arrowTargets[i] = new List<GameObject>();
        }

        foreach (wordMove WM in FindObjectsOfType<wordMove>())
        {
            Destroy(WM.gameObject);
        }

        nextTargetTime = Time.time + targetFrequencyStep * targetFrequencyMax - targetSpeed;

        switchAudio(true);
    }

    private void audioSetup()
    {
        if (songNum == -1)
        {
            songNum = Random.Range(0, songs.Length);
        } else
        {
            songNum++;
            if (songNum >= songs.Length)
            {
                songNum = 0;
            }
        }

        AudioSource AS = GetComponent<AudioSource>();
        AS.clip = songs[songNum];
        switchAudio(false);
        AS.Play();

        targetFrequencyMax = maxStep[songNum];

        float speedMultiplier = bpm[songNum] / (60); //bps

        //make float multiplier below smaller to increase difficulty
        targetSpeed = 0.75f / speedMultiplier;//Seconds for target to travel entire height
        targetFrequencyStep = speedMultiplier / targetFrequencyMax; // # of second per beat
        perfectDistRatio = 0.25f / speedMultiplier;//Ratio of target size that counts as perfect

        nextTargetTime = Time.time + targetFrequencyStep * targetFrequencyMax - targetSpeed;
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

    private void switchAudio(bool on)
    {
        foreach (AudioSource FAS in FindObjectsOfType<AudioSource>())
        {
            if (on)
            {
                FAS.Play();
                AS.clip = null;
            } else
            {
                FAS.Pause();
            }
        }
    }

    private void createWord(int i)
    {
        GameObject g = Instantiate(textPrefab, transform.parent);
        g.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-Screen.width/3, Screen.width/3), Random.Range(-Screen.height/3, Screen.height/3), 0);
        g.GetComponent<Image>().sprite = qualityImages[Mathf.Min(Mathf.Max(i, 0), 7)];
        Destroy(g, 8f);
    }

}
