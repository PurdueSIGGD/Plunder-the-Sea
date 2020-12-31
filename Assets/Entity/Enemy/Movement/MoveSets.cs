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

    public enum CheckTypes
    {
        Ray,
        Point
    }

    public static bool checkMove(CheckTypes type, Vector2 start, MoveAction move, ContactFilter2D filter)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];

        return Physics2D.Raycast(start, move.dir, filter, hits, move.dist + .1f) <= 0;
    }

    public static MoveAction[] getDirections(MoveTypes[] types)
    {
        List<MoveAction> ret = new List<MoveAction>();
        foreach (MoveTypes type in types)
        {
            ret.AddRange(getDirections(type));
        }

        return ret.ToArray();
    }

    public static MoveAction[] getDirections(MoveTypes type)
    {
        //MoveAction move = new MoveAction(Vector2.up, 1);
        switch (type)
        {
            case MoveTypes.Rook:
                return new MoveAction[]
                {
                    new MoveAction(Vector2.up, 1),
                    new MoveAction(Vector2.down, 1),
                    new MoveAction(Vector2.left, 1),
                    new MoveAction(Vector2.right, 1)
                };
            case MoveTypes.Bishop:
                float dist = Mathf.Sqrt(2);
                return new MoveAction[]
                {
                    new MoveAction(Vector2.left+Vector2.up, dist),
                    new MoveAction(Vector2.left+Vector2.down, dist),
                    new MoveAction(Vector2.right+Vector2.up, dist),
                    new MoveAction(Vector2.right+Vector2.down, dist)
                };
            default:
                return new MoveAction[] { };
        }
    }
}
