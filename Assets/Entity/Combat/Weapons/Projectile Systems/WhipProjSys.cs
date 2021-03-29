using System.Collections.Generic;
using UnityEngine;

public class WhipProjSys : ProjectileSystem {

    private GameObject WhipLink = null;
    //Link creation frequency in seconds
    public float linkSpeed = 0.05f;
    private List<GameObject> links = new List<GameObject>();
    private UI_Camera cam;

    protected override void OnEquip(WeaponInventory inv)
    {
        this.linkSpeed = this.tables.whipLinkSpeed.get(this.weaponClass).Value;
        WhipLink = tables.whipLinkObj.WhipOBJ;
        this.cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    public override void Run(Projectile proj)
    {
        //If time to add another link
        if (proj.currentLifeTime / linkSpeed > links.Count)
        {
            Vector3 mousePos = cam.GetMousePosition();
            Vector3 linkPos = proj.transform.position;
            //Set position at end of last link
            if (links.Count > 0)
            {
                GameObject lastLink = links[links.Count - 1];
                Vector3 distance = new Vector3(lastLink.GetComponentInChildren<SpriteRenderer>().bounds.size.x, 0, 0);
                distance = lastLink.transform.rotation * distance;
                linkPos = lastLink.transform.position + distance;
            }
            //Shoot link as stationary projectile
            Projectile newLink = Projectile.Shoot(WhipLink, linkPos, mousePos);
            newLink.source = proj.source;
            newLink.transform.SetParent(proj.transform);
            links.Add(newLink.gameObject);
        }
    }


    public override void OnEnd(Projectile projectile)
    {
        //Remove all links
        foreach (GameObject link in links) {
            Destroy(link);
        }
        links.Clear();
    }

    static WhipProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => (t.whipLinkSpeed.get(c).HasValue) ? new WhipProjSys() : null
        );
}