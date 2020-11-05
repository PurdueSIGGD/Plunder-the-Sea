using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using UnityEngine;

// Utility subclass for state-driven enemy movement
public class StateMovement : EnemyMovement
{
    

    public LinkedList<MoveAction> moveActions;

    // One-liner for distance from the player
    protected float PlayerDistance()
    {
        return Vector2.Distance(myBase.player.transform.position, myBase.transform.position);
    }

    // Move towards the player
    protected void MoveTowards()
    {
        

        Hashtable pathMap = new Hashtable();
        moveActions = new LinkedList<MoveAction>();
        Queue<PathAction> frontier = new Queue<PathAction>();
        Vector2 myPos = centerVector(myBase.myRigid.position);
        Vector2 playerPos = centerVector(myBase.player.transform.position);
        pathMap.Add(myPos, true);
        frontier.Enqueue(new PathAction(new Vector2(myPos.x, myPos.y), null));
        int maxDist = 10;
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(1 << 8);
        Debug.Log("Start: " + myPos);

        while (frontier.Count > 0)
        {
            PathAction curr = frontier.Dequeue();
            if (Mathf.Abs(curr.pos.x - playerPos.x) <= .1 && Mathf.Abs(curr.pos.y-playerPos.y) <= .1)
            {
                //Build path
                while (curr.parent != null)
                {
                    moveActions.AddFirst(new MoveAction(curr.pos-curr.parent.pos, 1));
                    curr = curr.parent;
                }
                moveActions.AddLast(new MoveAction(2*((Vector2)myBase.player.transform.position - myBase.myRigid.position), 1));
                break;
            }
            if (Vector2.Distance(curr.pos, playerPos) < maxDist)
            {
                explorePath(Vector2.up, curr, frontier, pathMap, filter);
                explorePath(Vector2.right, curr, frontier, pathMap, filter);
                explorePath(Vector2.down, curr, frontier, pathMap, filter);
                explorePath(Vector2.left, curr, frontier, pathMap, filter);
            }
            
            
        }

        if (moving && moveActions.Count > 0)
        {
            //myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
            //    * myBase.myStats.movementSpeed;
            Vector2 correction = .5f * (myPos - myBase.myRigid.position);

            myBase.myRigid.velocity = (moveActions.First.Value.dir+correction).normalized * myBase.myStats.movementSpeed;
            string output = "Path: "+ myBase.myRigid.velocity;
            Debug.Log(output);
        }
        else
        {
            myBase.myRigid.velocity = Vector2.zero;
        }

    }

    private void explorePath(Vector2 dir, PathAction parent, Queue<PathAction> front, Hashtable map, ContactFilter2D filter)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        Vector2 newPos = parent.pos + dir;
        if (!map.Contains(newPos))
        {
            if (Physics2D.Raycast(parent.pos, dir, filter, hits, 1.1f) <= 0)
            {
                map.Add(newPos, true);
                front.Enqueue(new PathAction(newPos, parent));
            }
            else
            {
                Debug.Log("Blocked: "+parent.pos+", "+dir);
            }
        }
    }

    private Vector2 centerVector(Vector3 vec)
    {
        return new Vector2(Mathf.Floor(vec.x)+.5f, Mathf.Floor(vec.y)+.5f);
    }

    // Move away from the player
    protected void MoveAway()
    {
        if (moving)
        {
            myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
                * -myBase.myStats.movementSpeed;
        }
    }

    // The target time for changing actions, used for time-driven states (charging up)
    private float targetTime = 0.0f;

    // Set a new target time
    protected void SetTarget(float delay)
    {
        targetTime = Time.time + delay;
    }

    // Check if the target time has passed
    protected bool OnTarget()
    {
        return (Time.time >= targetTime);
    }

    // Get the current state (overridden in subclasses)
    public virtual int GetState()
    {
        return 0;
    }
}

public class MoveAction
{
    public Vector2 dir;
    public float dist;

    public MoveAction(Vector2 dir, float dist)
    {
        this.dir = dir;
        this.dist = dist;
    }
}

public class PathAction
{
    public Vector2 pos;
    public PathAction parent;

    public PathAction(Vector2 pos, PathAction parent)
    {
        this.pos = pos;
        this.parent = parent;
    }
}