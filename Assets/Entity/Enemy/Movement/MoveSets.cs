using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSets
{
    // Start is called before the first frame update
    public enum MoveTypes
    {
        Rook,
        Bishop
    }

    public static Vector2[] getDirections(MoveTypes[] types)
    {
        List<Vector2> ret = new List<Vector2>();
        foreach (MoveTypes type in types)
        {
            ret.AddRange(getDirections(type));
        }

        return ret.ToArray();
    }

    public static Vector2[] getDirections(MoveTypes type)
    {
        switch (type)
        {
            case MoveTypes.Rook:
                return new Vector2[]
                {
                    Vector2.up,
                    Vector2.down,
                    Vector2.left,
                    Vector2.right
                };
            case MoveTypes.Bishop:
                return new Vector2[]
                {
                    Vector2.left+Vector2.up,
                    Vector2.left+Vector2.down,
                    Vector2.right+Vector2.up,
                    Vector2.right+Vector2.down
                };
            default:
                return new Vector2[] { };
        }
    }
}
