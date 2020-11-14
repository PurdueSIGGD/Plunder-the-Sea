using UnityEngine;

public class SpeedProjSys : ProjectileSystem {

    private float speed;

    public SpeedProjSys(float speed) {
        this.speed = speed;
    }

    public override void OnFire(Projectile projectile)
    {
        Rigidbody2D rigidBody = projectile.GetComponent<Rigidbody2D>();

        if (rigidBody)
        {
            rigidBody.velocity = projectile.direction * speed;
        }

    }
}