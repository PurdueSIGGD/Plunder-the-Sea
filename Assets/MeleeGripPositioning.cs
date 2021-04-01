using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGripPositioning : MonoBehaviour
{
    private UI_Camera cam;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Transform center;
    void Start()
    {
        cam = GameObject.FindObjectOfType<UI_Camera>();
    }
    public void Update() {
        var gunDir = ((Vector3)(cam.GetMousePosition() - (Vector2)center.position)).normalized;
        transform.localPosition = gunDir * radius;
    }
}
