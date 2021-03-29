using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform leftTransform;
    [SerializeField]
    private Transform rightTransform;
    [SerializeField]
    private WeaponInventory weaponInventory;
    [SerializeField]
    private PlayerAnimator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }

    private UI_Camera cam;

    private void LateUpdate()
    {
        var isSpriteFlipped = playerAnimator.getSpriteFlip();
        var spriteRen = GetComponent<SpriteRenderer>();
        spriteRen.sprite = weaponInventory.GetGunSprite();
        transform.position = (isSpriteFlipped)? leftTransform.position : rightTransform.position;
        var gunDir = ((Vector3)(cam.GetMousePosition() - (Vector2)transform.position)).normalized;
        spriteRen.flipX = gunDir.x < 0;

        var animGunDir = gunDir;
        if (gunDir.x < 0) {
            animGunDir.x = gunDir.y;
            animGunDir.y = -gunDir.x;
        }

        transform.rotation = Quaternion.FromToRotation(new Vector3(1, 1, 0), animGunDir);
    }
}
