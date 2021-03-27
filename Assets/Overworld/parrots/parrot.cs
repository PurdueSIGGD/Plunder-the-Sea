using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parrot : MonoBehaviour
{
    //if in range of player -> flies randomly for a time then rests

    //if out of range of player -> 
    //if resting -> wake up after a second then follow player
    //if flying -> folow player
    
    private Transform playerTransform;
    [SerializeField]
    private float maxDist = 1;
    [SerializeField]
    private float followSpeed = 2;
    [SerializeField]
    private float idleSpeed = 1;
    private Vector2 target;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer renderer;

    private float restTimer;
    private float wakeTimer;
    [SerializeField]
    private float timeToRest = 2;
    [SerializeField]
    private float timeToWake = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerBase>().transform;
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetFloat("Parrot", PlayerPrefs.GetFloat("classNum"));
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform)
        {
            if (checkBounds())
            {
                if (restTimer < timeToRest)
                {
                    restTimer += Time.deltaTime;
                    //fly randomly
                    rigid.drag = 1;
                    rigid.AddForce((playerTransform.position + Random.insideUnitSphere * maxDist - transform.position) * idleSpeed);
                }
                else
                {
                    //rest
                    if (wakeTimer > 0)
                    {
                        wakeTimer = 0;
                    }
                    anim.SetBool("Landed", true);
                    rigid.drag = 4;
                }
            }
            else
            {
                if (restTimer < timeToRest)
                {
                    if (restTimer > 0)
                    {
                        restTimer = 0;
                    }
                    //follow
                    rigid.AddForce((playerTransform.position - transform.position) * followSpeed);
                }
                else
                {
                    //wake up after a time
                    wakeTimer += Time.deltaTime;
                    if (wakeTimer > timeToWake)
                    {
                        restTimer = 0;
                        wakeTimer = 0;
                        anim.SetBool("Landed", false);
                    }
                }
            }
        }
        else
        {
            playerTransform = FindObjectOfType<PlayerBase>().transform;
        }
        anim.SetFloat("Speed", rigid.velocity.magnitude);
        if (rigid.velocity.x > 0)
        {
            renderer.flipX = false;
        }
        if (rigid.velocity.x < 0)
        {
            renderer.flipX = true;
        }
    }

    private bool checkBounds()
    {
        return Vector2.Distance(playerTransform.position, transform.position) <= maxDist;
    }
}
