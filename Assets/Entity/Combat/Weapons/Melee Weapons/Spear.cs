using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableWeapon", menuName = "ScriptableObjects/Spear", order = 1)]
public class Spear : ScriptableWeapon
{

    private List<Projectile> projList = new List<Projectile>();

    public override void Update()
    {
        foreach (Projectile proj in projList)
        {
            Vector3 dir = proj.transform.rotation * Vector3.right;
            if (proj.currentLifeTime >= proj.lifeTime / 2)
            {
                dir *= -1;
            }
            SpriteRenderer sprite = proj.GetComponentInChildren<SpriteRenderer>();
            float delta = Time.deltaTime / proj.lifeTime;
            float magnitude = sprite.bounds.size.x;
            proj.transform.localPosition += dir * delta * magnitude;
        }
    }

    public override void OnFire(Projectile projectile)
    {
        projList.Add(projectile);
    }

    public override void OnEnd(Projectile projectile)
    {
        projList.Remove(projectile);
    }

}
