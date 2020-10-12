using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bobber : MonoBehaviour
{

    private bool casting = true;
    private bool reeling = false;
    private PlayerFishing source;
    private Rigidbody2D rigid;

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
            return false;
        }

        reeling = true;
        return true;
    }

    private void OnTriggerStay2D(Collider2D collider)
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
            if (fish)
            {
                fish.BuffPlayerStats(source.player.stats);
                source.OnReelFinish(fish);
                Destroy(fish.gameObject);
                Destroy(this.gameObject);
                return;
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

        return obj;

    }
}
