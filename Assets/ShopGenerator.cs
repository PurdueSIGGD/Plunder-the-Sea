using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject shopItemPrefab;

    [SerializeField]
    private Transform[] targets;
    private bool used = false;
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag != "Player") {;
            return;
        }

        if (used)
        {
            return;
        }
        used = true;
        var weaponClasses = (WeaponFactory.CLASS[])System.Enum.GetValues(typeof(WeaponFactory.CLASS));
        foreach (var target in targets)
        {
            var randWClass = weaponClasses[Random.Range(0, weaponClasses.Length)];

            //bottle check
            if (randWClass == WeaponFactory.CLASS.BOTTLE)
            {
                continue;
            }

            var itemGameObj = Instantiate(shopItemPrefab, target.position, Quaternion.identity);
            var itemGameComp = itemGameObj.GetComponent<ShopItem>();

            itemGameComp.SetShopItem(randWClass, Random.Range(1, 11), Random.Range(0, 4));
            itemGameComp.OnEnable();

        }

        this.enabled = false;
    }
}
