using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    public GameObject bobberSprite;
    public float castingSpd;
    bool casted = false;
    public bool inWater = false;
    GameObject bobber;

    private void Update()
    {
        if (Input.GetButtonDown("Cast Fishing Pole"))   // F key
        {
            if (casted == false)
                StartCoroutine(Cast());
            else
                StartCoroutine(ReelIn());
        }
    }

    // casts the bobber to the point of the mouse
    public IEnumerator Cast()
    {
        casted = true;
        bobber = Instantiate(bobberSprite, transform.position, Quaternion.identity);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * castingSpd;
            bobber.transform.position = Vector2.Lerp(transform.position, mousePos, t);
            yield return null;
        }
        inWater = true;
    }

    // returns the bobber to the player position and destroys it
    public IEnumerator ReelIn()
    {
        inWater = false;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * castingSpd;
            bobber.transform.position = Vector2.Lerp(bobber.transform.position, transform.position, t);
            yield return null;
        }
        casted = false;
        Destroy(bobber);
    }
}
