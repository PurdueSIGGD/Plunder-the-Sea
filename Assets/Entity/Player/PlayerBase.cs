using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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
    [SerializeField]
    private bool keep = true;

    private UI_Camera cam;
    
    public void moveHere(Transform newPos)
    {
        this.transform.position = newPos.position;
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

        /* Assume one camera exists */
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    private void Update()
    {
        var inv = GetComponent<WeaponInventory>();
        

        if (Input.GetButtonDown("Fire1"))
        {
            inv.ShootAt(cam.GetMousePosition(), false);
        }

        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Space))
        {
            inv.ShootAt(cam.GetMousePosition(), true);
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

        if (Input.GetKeyDown("q"))
        {
            classUlt.callUlt(GetComponent<PlayerClasses>().classNumber);
        }
    }

    
    //PlayerStats calls this when player kills entity
    public void OnKill (EntityStats victim) 
    {
        GetComponent<WeaponInventory>().OnKill(victim);
           
    }

   
}
