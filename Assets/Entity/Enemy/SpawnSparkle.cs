using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSparkle : MonoBehaviour
{
    public GameObject sparkle;  // Sparkle object to be used
    public Color sparkleColor;  // Sprite color to color the sparkles
    public float rate;          // Number of sparkles per second
    public float size;          // Radius to display the sparkles on
    public bool rainbow = false;       // Whether the sparkles are random colors

    private float next = 0;

    // Update is called once per frame
    void Update()
    {
        // Spawn the sparkle every (next) seconds
        if (Time.time > next)
        {
            next = Time.time + rate;
            Vector3 offset = new Vector3(Random.Range(-size, size), Random.Range(-size, size), 0);
            GameObject newSparkle = Instantiate(sparkle, transform.position + offset, Quaternion.identity, transform);
            if (rainbow)
            {
                float r = Random.Range(0f, 255f) / 255f;
                float g = Random.Range(0f, 255f) / 255f;
                float b = Random.Range(0f, 255f) / 255f;
                Debug.Log(new Color(r, g, b));
                newSparkle.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
            }
            else
            {
                newSparkle.GetComponent<SpriteRenderer>().color = sparkleColor;
            }
            newSparkle.transform.localScale = new Vector3(1, 1, 0);
        }
    }
}
