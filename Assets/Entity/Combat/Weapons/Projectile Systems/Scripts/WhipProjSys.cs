using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WhipProjSys", menuName = "ScriptableObjects/ProjectileSystems/Whip", order = 1)]
public class WhipProjSys : ProjectileSystem {

    [SerializeField]
    private GameObject WhipLink = null;
    //Link creation frequency in seconds
    public float linkSpeed = 0.05f;
    private List<GameObject> links = new List<GameObject>();
    private UI_Camera cam;

    public WhipProjSys(float linkSpeed) {
        this.linkSpeed = linkSpeed;
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

    public override void OnEquip(WeaponInventory inv)
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    public override void OnEnd(Projectile projectile)
    {
        //Remove all links
        foreach (GameObject link in links) {
            Destroy(link);
        }
        links.Clear();
    }

}