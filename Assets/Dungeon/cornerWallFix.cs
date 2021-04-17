using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cornerWallFix : MonoBehaviour
{
    //[Header("1 = create offsetted box right, 2 = create offsetted box both, 3 = expand box")]

    [Header("Size = 4, 0 = nothing, 1 = expand box")]

    [SerializeField]
    private int[] orientationActions;

    // Update is called once per frame
    public void fixCorner()
    {
        float rotation = transform.eulerAngles.z % 360;
        int action = orientationActions[(int)(rotation / 90)];
        if (action == 1)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.offset = Vector2.zero;
            box.size = Vector2.one;
        }
        if (action == 2)
        {
            BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
            box.offset = Quaternion.Euler(0, 0, rotation) * Vector2.left;
        }
    }
}
