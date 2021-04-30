using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerFishing))]
public class PlayerBase : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public PlayerFishing fishing;
    [HideInInspector]
    public ClassUltimate classUlt;
    public Canvas playerInventory;
    [HideInInspector]
    public PlayerMenu helpMenu;
    [SerializeField]
    private bool keep = true;

    private bool fadeIn = true;
    [SerializeField]
    private Image fader;
    [SerializeField]
    private GameObject fadeCanvas;
    [SerializeField]
    private Text levelText;
    private float mainMenuTimer = 0;
    private float quitTimer = 0;

    private UI_Camera cam;
    
    public Vector2 getCamMousePos()
    {
        return cam.GetMousePosition();
    }

    public void moveHere(Transform newPos)
    {
        this.transform.position = newPos.position;
        StartCoroutine("loadIn");
    }

    public IEnumerator loadIn()
    {
        fadeIn = true;
        fadeCanvas.SetActive(true);
        fader.color = Color.black;
        if (keep)
        {
            if (!stats)
            {
                stats = GetComponent<PlayerStats>();
            }
            if (!FindObjectOfType<yPositionLayering>()) {
                levelText.text = string.Format("Level  {0}", stats.dungeonLevel + 1);
            } else
            {
                levelText.text = string.Format("Overworld  {0}", stats.dungeonLevel + 1);
            }
        } else {
            levelText.text = "Tutorial";
        }
        yield return new WaitForSeconds(5f);
        fadeCanvas.SetActive(false);
    }

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 1)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerBase>().moveHere(this.transform);
            }
            Destroy(this.gameObject);
        } else
        {
            StartCoroutine("loadIn");
        }
        if (keep)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        fishing = GetComponent<PlayerFishing>();
        classUlt = GetComponent<ClassUltimate>();
        helpMenu = GetComponentInChildren<PlayerMenu>();
        helpMenu.frame.SetActive(false);

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    private void Update()
    {
        var inv = GetComponent<WeaponInventory>();
        
        if (fadeIn)
        {
            fader.color = new Color(0, 0, 0, (fader.color.a-Time.deltaTime/3));
            if (fader.color.a <= 0)
            {
                fadeIn = false;
            }
        }

        if (stats.actionLock <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                inv.ShootAt(cam.GetMousePosition(), false);
            }

            if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Space))
            {
                inv.ShootAt(cam.GetMousePosition(), true);
            }

            if (Input.GetKeyDown("q"))
            {
                classUlt.callUlt(GetComponent<PlayerClasses>().classNumber);
            }
        }

        if (Input.GetKeyDown("e"))
        {
            if (playerInventory.gameObject.activeSelf)
            {
                playerInventory.gameObject.SetActive(false);
            }
            else
            {
                playerInventory.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown("h"))
        {
            if (helpMenu.frame.activeSelf)
            {
                helpMenu.frame.SetActive(false);
            }
            else
            {
                helpMenu.frame.SetActive(true);
            }
        }
        if (Input.GetKeyDown("p"))
        {
            if (helpMenu.tip.activeSelf)
            {
                helpMenu.tip.SetActive(false);
            }
            else
            {
                helpMenu.tip.SetActive(true);
            }
        }

        //quits the game
        if (Input.GetKey(KeyCode.Escape))
        {
            mainMenuTimer += Time.deltaTime;
            if (mainMenuTimer >= 2)
            {
                Application.Quit();
            }
        }
        else
        {
            mainMenuTimer = 0;
        }
        //goes to the menu
        if (GetComponent<debugging>().debug && Input.GetKey(KeyCode.M))
        {
            quitTimer += Time.deltaTime;
            if (quitTimer >= 2)
            {
                Destroy(gameObject, 0.1f);
                Destroy(Camera.main.gameObject, 0.1f);
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            quitTimer = 0;
        }
    }

    
    //PlayerStats calls this when player kills entity
    public void OnKill (EntityStats victim) 
    {
        GetComponent<WeaponInventory>().OnKill(victim);
           
    }

   
}
