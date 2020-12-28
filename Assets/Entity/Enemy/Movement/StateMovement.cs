using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using UnityEngine;

// Utility subclass for state-driven enemy movement
public class StateMovement : EnemyMovement
{

    public MoveSets.MoveTypes[] moveTypes = { MoveSets.MoveTypes.Rook, MoveSets.MoveTypes.Bishop };
    public LinkedList<MoveAction> moveActions;
    public float pathingRefresh = .25f;
    public float maxDist = 10;
    public float targetProximity = 0;
    private float lastRefresh = -Mathf.Infinity;
    private int actionLock = 0;

    // One-liner for distance from the player
    public float PlayerDistance()
    {
        return Vector2.Distance(myBase.player.transform.position, myBase.transform.position);
    }

    // Move towards the player
    protected void MoveTowards()
    {
        float nowSecs = Time.time;
        if (nowSecs - lastRefresh >= pathingRefresh && actionLock <= 0) {
            lastRefresh = nowSecs;
            if (calcPath())
            {
                actionLock = 1;
            }
        }

        if (moving && moveActions.Count > 0)
        {
            //myBase.myRigid.velocity = (myBase.player.transform.position - this.transform.position).normalized
            //    * myBase.myStats.movementSpeed;
            Vector2 correction = .5f * (centerVector(myBase.myRigid.position) - myBase.myRigid.position);

            MoveAction currAction = moveActions.First.Value;
            if (currAction.dist <= 0)
            {
                moveActions.RemoveFirst();
                actionLock--;
                if (moveActions.Count > 0)
                {
                    currAction = moveActions.First.Value;
                }
            }
            if (moveActions.Count > 0)
            {
                currAction.dist -= Time.deltaTime * myBase.myStats.movementSpeed;
                myBase.myRigid.velocity = (moveActions.First.Value.dir + correction).normalized * myBase.myStats.movementSpeed;
            }
        }
        if (moving && moveActions.Count <= 0)
        {
            if (Vector3.Distance(myBase.myRigid.position, myBase.player.transform.position) <= 2.5f)
            {
                myBase.myRigid.velocity = ((Vector2)myBase.player.transform.position - myBase.myRigid.position).normalized * myBase.myStats.movementSpeed;
            }
            else
            {
                myBase.myRigid.velocity = Vector2.zero;
            }
        }

    }

    public bool calcPath()
    {
        Hashtable pathMap = new Hashtable();
        LinkedList<MoveAction> moveActionsBuild = new LinkedList<MoveAction>();
        //Queue<PathAction> frontier = new Queue<PathAction>();
        PriorityQueue frontier = new PriorityQueue();
        Vector2 myPos = centerVector(myBase.myRigid.position);
        Vector2 playerPos = centerVector(myBase.player.transform.position);
        PathAction startAct = new PathAction(new Vector2(myPos.x, myPos.y), null);
        pathMap.Add(myPos, startAct);
        //frontier.Enqueue(startAct);
        frontier.Push(startAct);
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(1 << 8);
        //Debug.Log("Start: " + myPos);

        while (frontier.Count() > 0)
        {
            PathAction curr = frontier.Pop();
            if (Mathf.Abs(curr.pos.x - playerPos.x) <= targetProximity+.1 && Mathf.Abs(curr.pos.y - playerPos.y) <= targetProximity+.1)
            {
                while (curr.parent != null)
                {
                    //Debug.Log(curr.pos + " value: "+curr.getValue() + " dist: " + curr.dist);
                    moveActionsBuild.AddFirst(new MoveAction(curr.pos - curr.parent.pos, (curr.pos - curr.parent.pos).magnitude));
                    curr = curr.parent;
                }
                break;
            }
            if (Vector2.Distance(curr.pos, playerPos) < maxDist)
            {
                foreach (Vector2 direction in MoveSets.getDirections(moveTypes))
                {
                    explorePath(direction, playerPos, curr, frontier, pathMap, filter);
                }
                //explorePath(Vector2.right + Vector2.up, playerPos, curr, frontier, pathMap, filter);
                //explorePath(Vector2.right + Vector2.down, playerPos, curr, frontier, pathMap, filter);
                //explorePath(Vector2.left + Vector2.up, playerPos, curr, frontier, pathMap, filter);
                //explorePath(Vector2.left + Vector2.down, playerPos, curr, frontier, pathMap, filter);

                //explorePath(Vector2.up, playerPos, curr, frontier, pathMap, filter);
                //explorePath(Vector2.right, playerPos, curr, frontier, pathMap, filter);
                //explorePath(Vector2.down, playerPos, curr, frontier, pathMap, filter);
                //explorePath(Vector2.left, playerPos, curr, frontier, pathMap, filter);
            }


        }
        moveActions = moveActionsBuild;
        if (moveActionsBuild.Count > 0)
        {
            return true;
        }
        return false;
    }

    private void explorePath(Vector2 dir, Vector2 target, PathAction parent, PriorityQueue front, Hashtable map, ContactFilter2D filter)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        Vector2 newPos = parent.pos + dir;
        //Debug.Log(parent.pos + " -> " + newPos);
        if (!map.Contains(newPos))
        {
            if (Physics2D.Raycast(parent.pos, dir, filter, hits, dir.magnitude+.1f) <= 0)
            {
                PathAction newAct = new PathAction(newPos, parent, target);
                map.Add(newPos, newAct);
                front.Push(newAct);
            }
        }
        else
        {
            //check if should update value in frontier
            if (Physics2D.Raycast(parent.pos, dir, filter, hits, dir.magnitude+.1f) <= 0)
            {
                PathAction oldAct = (PathAction)map[newPos];
                if (parent.dist + dir.magnitude < oldAct.dist)
                {
                    oldAct.parent = parent;
                    front.Update(oldAct, parent.dist + dir.magnitude);
                }
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

    // Set the current state (overridden in subclasses)
    public virtual void SetState(int newState)
    {
        return;
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

public class PriorityQueue
{
    public List<PathAction> queue;

    public PriorityQueue()
    {
        queue = new List<PathAction>();
    }

    public int Count()
    {
        return queue.Count;
    }

    public void Update(PathAction act, float newDist)
    {
        //binary search to find action in queue
        int high = queue.Count - 1;
        float highVal = queue[high].getValue();
        int low = 0;
        float lowVal = queue[low].getValue();
        float actVal = act.getValue();

        while (highVal > lowVal)
        {
            int mid = (high + low) / 2;
            float midVal = queue[mid].getValue();
            if (midVal >= actVal)
            {
                high = mid;
                highVal = midVal;
            }
            else
            {
                low = mid+1;
                lowVal = queue[low].getValue();
            }
        }

        int curr = low;
        float currVal = lowVal;
        while (currVal <= highVal && curr < queue.Count)
        {
            if (act == queue[curr])
            {
                queue.RemoveAt(curr);
                break;
            }
            curr++;
            currVal = queue[curr].getValue();
        }

        act.dist = newDist;
        Push(act);
    }

    public void Push(PathAction act)
    {
        if (queue.Count <= 0)
        {
            queue.Add(act);
            return;
        }
        int high = queue.Count-1;
        int low = 0;
        float actVal = act.getValue();

        //Binary search for insertion location
        while (high - low > 1)
        {
            int mid = (high + low) / 2;
            if (queue[mid].getValue() > actVal)
            {
                high = mid;
            }
            else
            {
                low = mid;
            }
        }
        if (queue[low].getValue() > actVal)
        {
            queue.Insert(low, act);
        }
        else if (queue[high].getValue() < actVal)
        {
            queue.Insert(high + 1, act);
        }
        else
        {
            queue.Insert(low + 1, act);
        }
    }

    public PathAction Peek()
    {
        return queue[0];
    }
    public PathAction Pop()
    {
        PathAction ret = queue[0];
        queue.RemoveAt(0);
        return ret;
    }

}

public class PathAction
{
    public Vector2 pos;
    public PathAction parent;
    public float dist = 0;
    public float heuristic = 0;

    public PathAction(Vector2 pos, PathAction parent, Vector2 target, float nextDist)
    {
        this.pos = pos;
        this.parent = parent;
        if (parent != null)
        {
            dist = parent.dist + nextDist;
        }
        heuristic = Vector2.Distance(pos, target);
    }

    public PathAction(Vector2 pos, PathAction parent, Vector2 target)
    {
        this.pos = pos;
        this.parent = parent;
        if (parent != null)
        {
            dist = parent.dist + Vector2.Distance(parent.pos, pos);
        }
        heuristic = Vector2.Distance(pos, target);
    }

    public PathAction(Vector2 pos, PathAction parent)
    {
        this.pos = pos;
        this.parent = parent;
        if (parent != null)
        {
            dist = parent.dist + Vector2.Distance(parent.pos, pos);
        }
    }

    public float getValue()
    {
        return dist + heuristic;
    }
}