using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5.0f;
    private Camera cam;

    private void Start()
    {
        /* Assuming one camera */
        cam = GameObject.FindObjectOfType<Camera>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            BulletManager.Shoot(projectilePrefab, gameObject, mouseWorldPos, projectileSpeed);

        }
    }
}
