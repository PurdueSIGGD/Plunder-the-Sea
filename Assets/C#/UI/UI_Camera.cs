using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UI_Camera : MonoBehaviour
{

    /* Target which camera follows */
    public GameObject target;
    /* Offset for targeting mode */
    public Vector2 offset;

    private Camera cam;
    private Vector2 panPosition;
    private float panEndTime = 0.0f;
    private bool isPanning = false;
    private float zoomSize = 1.0f;
    private float zoomEndTime = 0.0f;
    private bool isZooming = false;

    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        
        if (target)
        {
            SetPosition((Vector2)target.transform.position + offset);
        }
        else if (isPanning)
        {
            if (Time.time >= panEndTime)
            {
                isPanning = false;
                SetPosition(panPosition);
            }
            else
            {
                float delta = Time.deltaTime;
                float timeRemain = panEndTime - Time.time - delta;
                Vector2 speed = (panPosition - (Vector2)this.transform.position) / timeRemain;
                AddPosition(speed * delta);
            }
        }

        if (isZooming)
        {
            if (Time.time >= zoomEndTime)
            {
                isZooming = false;
                SetSize(zoomSize);
            }
            else
            {
                float delta = Time.deltaTime;
                float timeRemain = zoomEndTime - Time.time - delta;
                float speed = (zoomSize - GetSize()) / timeRemain;
                SetSize(GetSize() + speed * delta);
            }
        }

    }

    /*
     * Pan camera to a position over time.
     * Removes targeting.
     */
    public void PanTo(Vector2 position, float seconds)
    {

        target = null;
        isPanning = true;
        panPosition = position;
        panEndTime = Time.time + seconds;

    }

    public void SetPosition(Vector2 position)
    {
        /* Keep z-value the same */
        this.gameObject.transform.position = new Vector3(position.x, position.y, this.gameObject.transform.position.z);
    }

    /* Set camera position relatively */
    public void AddPosition(Vector2 displace)
    {
        SetPosition((Vector2)this.gameObject.transform.position + displace);
    }

    public bool IsPanning()
    {
        return isPanning;
    }

    public float GetSize()
    {
        return cam.orthographicSize;
    }

    public void SetSize(float amt)
    {
        cam.orthographicSize = amt;
    }

    /* Zoom camera instantly */
    public void Zoom(float scale)
    {
        SetSize(GetSize() / scale);
    }

    /* Zoom camera over time */
    public void Zoom(float scale, float seconds)
    {
        isZooming = true;
        zoomSize = GetSize() / scale;
        zoomEndTime = Time.time + seconds;
    }

    public void Rotate(float angle)
    {
        SetRotation(GetRotation() + angle);
    }

    public void SetRotation(float angle)
    {
        cam.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public float GetRotation()
    {
        return cam.transform.rotation.eulerAngles.z;
    }

    public Vector2 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }

}
