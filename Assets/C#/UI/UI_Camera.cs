using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Camera : MonoBehaviour
{

    /* Target which camera follows */
    public GameObject target = null;
    /* Offset for targeting mode */
    public Vector2 offset;

    private Vector2 panPosition;
    private float panEndTime = 0.0f;
    private bool isPanning = false;

    private void Start()
    {
        PanTo(new Vector2(-5, -5), 10);
    }

    void Update()
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
        panEndTime = seconds;

    }

    /* Set camera position */
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

}
