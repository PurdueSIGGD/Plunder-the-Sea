using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bobber : MonoBehaviour
{

    public bool casting = true;
    private bool reeling = false;
    public PlayerFishing source;
    private Rigidbody2D rigid;
    public int baitType;
    private bool hitFish = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Too far away, line breaks
        if (Vector2.Distance(source.transform.position, this.transform.position) > source.castingDistance)
        {
            source.OnReelFinish(null);
            Destroy(this.gameObject);
            return;
        }

        if (casting)
        {
            //Bobber has settled
            if (rigid.velocity.magnitude <= 0.1f)
            {
                casting = false;
            }
        }
        else if (reeling)
        {
            rigid.velocity = ((Vector2)(source.transform.position - transform.position)).normalized * source.castingSpeed;
        }
    }

    public bool Reel()
    {
        if (casting)
        {
            rigid.velocity = new Vector2(0.0f, 0.0f);
        }
        hitFish = false;
        reeling = true;
        return true;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        OnContact(collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnContact(collider);
    }

    private void OnContact(Collider2D collider)
    {
        if (reeling)
        {
            //Reeling bobber has returned
            if (collider.gameObject == source.gameObject)
            {
                source.OnReelFinish(null);
                Destroy(this.gameObject);
                return;
            }

            Fish fish = collider.GetComponent<Fish>();
            //Fish wrangled!
            if (fish && !hitFish)
            {
                if (fish.preferredBaitType == baitType || Random.Range(0f,1f) <= fish.wrongBaitCatchPercent) {
                    FishingMinigame fm = fish.FishingMinigame.GetComponent<FishingMinigame>();
                    fm.ddr.fishBeingCaught = fish;
                    fm.ddr.targetPlayer = source.player;
                    fm.fish.SetSourceImage(fish.sprite);
                    source.player.movement.enabled = false;
                    source.player.rigidBody.velocity = new Vector3(0, 0, 0);
                    source.OnReelFinish(fish);
                    Destroy(fish.gameObject);
                    Destroy(this.gameObject);
                    return;
                } else
                {
                    Debug.Log("No bite");
                    hitFish = true;
                }
            }

        }
    }

    public static Bobber Create(GameObject prefab, PlayerFishing source, Vector2 target, int baitType)
    {
        //baitType changes some conditions, but it is not programmed yet
        
        Bobber obj = Instantiate(prefab, source.transform.position, Quaternion.identity).GetComponent<Bobber>();
        Rigidbody2D rigid;

        if (obj && (rigid = obj.GetComponent<Rigidbody2D>()) )
        {
            obj.source = source;
            Vector2 velocity = (target - (Vector2)obj.transform.position).normalized * source.castingSpeed;
            rigid.velocity = velocity;
        }
        obj.baitType = baitType;

        return obj;

    }
}
