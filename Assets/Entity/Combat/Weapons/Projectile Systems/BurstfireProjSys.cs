using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstfireProjSys", menuName = "ScriptableObjects/ProjectileSystems/Burstfire", order = 1)]
public class BurstfireProjSys : ProjectileSystem {

    private int extraShots = 3;
    private float frequency = 0.05f;

    private int shotsNeeded = 0;
    private float nextShotTime = 0;
    private GameObject projTemplate;
    private Vector3 velocity = new Vector3();

    protected override void OnEquip(WeaponInventory inv)
    {
        this.extraShots = this.tables.burstFireExtraShots.get(this.weaponClass).Value;
        this.frequency = this.tables.burstFireFrequency.get(this.weaponClass).Value;
    }

    public override void OnFire(Projectile projectile)
    {
        Rigidbody2D origRigid = projectile.GetComponent<Rigidbody2D>();
        velocity = origRigid.velocity;
        shotsNeeded += extraShots;
        nextShotTime = Time.time + frequency;
        if (projTemplate)
        {
            Destroy(projTemplate);
        }
        projTemplate = Instantiate(projectile).gameObject;
        projTemplate.SetActive(false);
    }

    public override void Run(Projectile projectile)
    {
        //Time to shoot again
        if (shotsNeeded > 0 && Time.time > nextShotTime)
        {
            shotsNeeded--;
            nextShotTime = Time.time + frequency;
            GameObject newProj = Instantiate(projTemplate);
            newProj.SetActive(true);
            newProj.transform.position = projectile.source.transform.position;
            Rigidbody2D rigid = newProj.GetComponent<Rigidbody2D>();
            rigid.velocity = velocity;
        }
    }

    // only weapons with the burstfire stats can use this system
    static BurstfireProjSys() =>
        WeaponFactory.BindSystem(
            (c, t) => 
                (t.burstFireExtraShots.get(c).HasValue && t.burstFireFrequency.get(c).HasValue) 
                ? new BurstfireProjSys() : null
        );
}