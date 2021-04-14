using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2D;
    [SerializeField]
    private Animator bodyAnimator;
    [SerializeField]
    private SpriteRenderer bodyRenderer;

    private bool spriteFlip = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool getSpriteFlip() {
        return this.spriteFlip;
    }

    // Update is called once per frame
    void Update()
    {
        float rb2DSpeed = rb2D.velocity.magnitude;
        float MIN_CHANGE_SPEED = 0.05f;

        if (rb2D.velocity.x > MIN_CHANGE_SPEED) {
            spriteFlip = false;
        }

        if (rb2D.velocity.x < -MIN_CHANGE_SPEED) {
            spriteFlip = true;
        }

        bodyRenderer.flipX = spriteFlip;

        bodyAnimator.SetFloat("walk", rb2DSpeed);
    }
}
