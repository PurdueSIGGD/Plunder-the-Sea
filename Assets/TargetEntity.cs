using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetEntity : MonoBehaviour
{
    [SerializeField]
    private int hitCountMax;
    private int hitCount = 0;
    public UnityEvent deathEvent;

    private bool idle = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (idle) return;

        var proj = collider.GetComponent<Projectile>();

        if (proj?.ProjectileType() == 2) {
            hitCount += 1;
        }

        if (hitCount >= hitCountMax) {
            //Destroy(gameObject);
            GetComponent<SpriteRenderer>().enabled = false;
            idle = true;
            deathEvent.Invoke();
            Invoke("revive", 2f);
        }
    }

    void revive() {
        idle = false;
        GetComponent<SpriteRenderer>().enabled = true;
        hitCount = 0;
    }
}
