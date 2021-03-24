using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSets
{
    // Start is called before the first frame update
    public enum MoveTypes
    {
        Rook,
        Bishop,
        Knight,
        BishopTp
    }

    public enum CheckTypes
    {
        Ray,
        Point
    }

    public static void executeMove(MoveAction currAction, EnemyBase myBase, Vector2 correction)
    {
        switch (currAction.type)
        {
            case CheckTypes.Ray:
                myBase.myRigid.velocity = (currAction.dir + correction).normalized * myBase.myStats.movementSpeed;
                return;
            case CheckTypes.Point:
                if (currAction.dist <= 0)
                {
                    myBase.myCombat.BeforeTeleport();
                    myBase.myRigid.position = myBase.myRigid.position + (currAction.dir + correction).normalized * currAction.maxDist;
                    myBase.myRigid.velocity = Vector2.zero;
                    myBase.myCombat.AfterTeleport();
                }
                return;
        }
    }

    public static bool checkMove(CheckTypes type, Vector2 start, MoveAction move, ContactFilter2D filter)
    {     
        switch (type)
        {
            case CheckTypes.Ray:
                RaycastHit2D[] hits = new RaycastHit2D[1];
                return Physics2D.Raycast(start, move.dir, filter, hits, move.dist + .1f) <= 0;
            case CheckTypes.Point:
                Collider2D[] cols = new Collider2D[1];
                return (Physics2D.OverlapCircle(start + move.dir.normalized * move.dist, 0, filter, cols) <= 0
                    && Physics2D.OverlapCircle(start + move.dir.normalized * move.dist, 0) != null);
        }
        return false;
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
        float dist;
        float moveDist;
        switch (type)
        {
            case MoveTypes.Rook:
                moveDist = 1;
                return new MoveAction[]
                {
                    new MoveAction(moveDist * Vector2.up, 1),
                    new MoveAction(moveDist *Vector2.down, 1),
                    new MoveAction(moveDist *Vector2.left, 1),
                    new MoveAction(moveDist *Vector2.right, 1)
                };
            case MoveTypes.Bishop:
                dist = Mathf.Sqrt(2);
                moveDist = 1;
                return new MoveAction[]
                {
                    new MoveAction(moveDist *(Vector2.left+Vector2.up), dist),
                    new MoveAction(moveDist *(Vector2.left+Vector2.down), dist),
                    new MoveAction(moveDist *(Vector2.right+Vector2.up), dist),
                    new MoveAction(moveDist *(Vector2.right+Vector2.down), dist)
                };
            case MoveTypes.Knight:
                dist = Mathf.Sqrt(5);
                moveDist = 2;
                return new MoveAction[]
                {
                    new MoveAction(moveDist*(Vector2.left+2*Vector2.up), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(Vector2.left+2*Vector2.down), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(Vector2.right+2*Vector2.up), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(Vector2.right+2*Vector2.down), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(2*Vector2.left+Vector2.up), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(2*Vector2.left+Vector2.down), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(2*Vector2.right+Vector2.up), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(2*Vector2.right+Vector2.down), moveDist*dist, CheckTypes.Point)
                };
            case MoveTypes.BishopTp:
                dist = Mathf.Sqrt(2);
                moveDist = 2;
                return new MoveAction[]
                {
                    new MoveAction(moveDist*(Vector2.left+Vector2.up), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(Vector2.left+Vector2.down), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(Vector2.right+Vector2.up), moveDist*dist, CheckTypes.Point),
                    new MoveAction(moveDist*(Vector2.right+Vector2.down), moveDist*dist, CheckTypes.Point)
                    //new MoveAction(Vector2.up, 1, CheckTypes.Point),
                    //new MoveAction(Vector2.down, 1, CheckTypes.Point),
                    //new MoveAction(Vector2.left, 1, CheckTypes.Point),
                    //new MoveAction(Vector2.right, 1, CheckTypes.Point)
                };
            default:
                return new MoveAction[] { };
        }
    }
}
