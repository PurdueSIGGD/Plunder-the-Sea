using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update

    public const int ROOMWIDTH = 10;
    public const int ROOMDIST = 6;
    public GameObject room;
    public GameObject hall;
    public GameObject wall;
    public GameObject goal;
    public int truePathLength;
    public int maxBranchLength;
    public float branchFactor;

    private Hashtable roomGrid;
    private Stack<RoomData> roomStack;

    public class RoomData
    {
        public int x;
        public int y;
        public int rank;
        public int branchLength;
        public List<int> nextDirs;
        public bool[] connectDirs;
        public RoomData parent;

        public RoomData(int x, int y, int b, RoomData p)
        {
            this.x = x;
            this.y = y;
            branchLength = b;
            rank = p.rank + 1;
            nextDirs = new List<int>(new int[] { 0, 1, 2, 3 });
            parent = p;
            connectDirs = new bool[4];
        }

        public RoomData(int x, int y, int b)
        {
            this.x = x;
            this.y = y;
            branchLength = b;
            rank = 0;
            nextDirs = new List<int>(new int[] { 0, 1, 2, 3 });
            parent = null;
            connectDirs = new bool[4];
        }
    }

    void Start()
    {
        roomGrid = new Hashtable();
        roomStack = new Stack<RoomData>();
        generate();
    }

    // Update is called once per frame
    public void generate()
    {
        int length = 1;
        int x = 0;
        int y = 0;
        RoomData curr = new RoomData(0, 0, 0);
        roomGrid.Add((curr.x, curr.y), curr);
        roomStack.Push(curr);
        while (length < truePathLength)
        {
            int r = UnityEngine.Random.Range(0, curr.nextDirs.Count);
            int rawDir = curr.nextDirs[r];
            curr.nextDirs.RemoveAt(r);
            (int, int) dir = toDirection(rawDir);
            int dx = dir.Item1;
            int dy = dir.Item2;
            if (validNext(x, y, dx, dy))
            {
                RoomData nextRoom = new RoomData(x+dx, y+dy, 0, curr);
                roomGrid.Add((nextRoom.x, nextRoom.y), nextRoom);
                roomStack.Push(nextRoom);
                curr.connectDirs[rawDir] = true;
                nextRoom.connectDirs[(rawDir+2) % 4] = true;
                x = nextRoom.x;
                y = nextRoom.y;
                curr = nextRoom;
                length++;
            }
        }

        branch();

        while (roomStack.Count > 1)
        {
            buildRoom(roomStack.Pop());
        }

        //This is a janky implementation of placing a change scene door in the final room
        //Changes to the generation algorithm could break this and it is not very robust
        //Handling room spawns and customization will have to be implemented later
        RoomData lastRoom = roomStack.Pop();
        buildRoom(lastRoom);
        int roomScale = ROOMWIDTH + ROOMDIST;
        Object.Instantiate(goal, new Vector3(lastRoom.x * roomScale + .5f * ROOMWIDTH,
            lastRoom.y * roomScale + .5f * ROOMWIDTH, 0), Quaternion.identity);
    }

    public void branch()
    {
        RoomData[] temp = roomStack.ToArray();
        //roomStack.Clear();
        //Queue<RoomData> branchQueue = new Queue<RoomData>();
        Stack<RoomData> branchStack = new Stack<RoomData>();

        //for (int i = temp.Length-1; i > 0; i--)
        //{
        //    branchQueue.Enqueue(temp[i]);
        //}
        RoomData endRoom = roomStack.Pop();
        while (roomStack.Count > 0)
        {
            branchStack.Push(roomStack.Pop());
        }
        roomStack.Push(endRoom);
        //roomStack.Push(temp[0]);

        while (branchStack.Count > 0)
        {
            RoomData curr = branchStack.Pop();
            roomStack.Push(curr);
            if (curr.branchLength < maxBranchLength)
            {
                for (int i = 0; i < curr.nextDirs.Count; i++)
                {
                    (int, int) dirs = toDirection(curr.nextDirs[i]);
                    if (connectNext(curr, dirs.Item1, dirs.Item2) && UnityEngine.Random.Range(0, 1f) <= branchFactor)
                    {
                        //create branch in this direction
                        RoomData nextRoom;
                        int nx = curr.x + dirs.Item1;
                        int ny = curr.y + dirs.Item2;
                        if (validNext(curr.x, curr.y, dirs.Item1, dirs.Item2))
                        {
                            nextRoom = new RoomData(nx, ny, curr.branchLength + 1, curr);
                            roomGrid.Add((nx, ny), nextRoom);
                            branchStack.Push(nextRoom);
                        }
                        else
                        {
                            nextRoom = (RoomData)roomGrid[(nx, ny)];
                        }
                        curr.connectDirs[curr.nextDirs[i]] = true;
                        nextRoom.connectDirs[(curr.nextDirs[i] + 2) % 4] = true;
                    }
                }
            }
        }

    }

    public (int, int) toDirection(int r)
    {
        int dx = r % 2;
        int dy = 1 - dx;
        if (r > 1)
        {
            dx *= -1;
            dy *= -1;
        }
        return (dx, dy);
    }

    public void buildRoom(RoomData newRoom)
    {
        int roomScale = ROOMWIDTH + ROOMDIST;
        for (int i = 0; i < 4; i++)
        {
            (int, int) dir = toDirection(i);
            if (newRoom.connectDirs[i])
            {
                //spawn hallways
                (float, float) hallLoc = (newRoom.x + .5f * dir.Item1, newRoom.y + .5f * dir.Item2);
                if (!roomGrid.Contains(hallLoc))
                {
                    //Debug.Log("Hall: " + hallLoc);
                    roomGrid.Add(hallLoc, 1);
                    Quaternion rot;
                    if (dir.Item1 == 0)
                    {
                        rot = Quaternion.identity;
                    }
                    else
                    {
                        rot = Quaternion.Euler(0, 0, 90);
                    }
                    Object.Instantiate(hall, new Vector3(hallLoc.Item1 * roomScale + .5f * ROOMWIDTH,
                        hallLoc.Item2 * roomScale + .5f * ROOMWIDTH, 0), rot);
                }
            }
            else
            {
                //plug opening
                (float, float) wallLoc = (newRoom.x * roomScale + .5f * ((dir.Item1 + 1) * ROOMWIDTH - dir.Item1),
                    newRoom.y * roomScale + .5f * ((dir.Item2 + 1) * ROOMWIDTH - dir.Item2));
                Quaternion rot;
                if (dir.Item2 == 0)
                {
                    rot = Quaternion.identity;
                }
                else
                {
                    rot = Quaternion.Euler(0, 0, 90);
                }
                Object.Instantiate(wall, new Vector3(wallLoc.Item1, wallLoc.Item2, 0), rot);
            }
        }
        Debug.Log("Room: " + (newRoom.x,newRoom.y) + ", " + (newRoom.rank, newRoom.branchLength));
        Object.Instantiate(room, new Vector3(newRoom.x * roomScale, newRoom.y * roomScale, 0), Quaternion.identity);
    }

    public bool validNext(int x, int y, int dx, int dy)
    {
        return !roomGrid.Contains((x + dx, y + dy));
    }

    public bool connectNext(RoomData curr, int dx, int dy)
    {
        if (validNext(curr.x, curr.y, dx, dy))
        {
            return true;
        }
        RoomData next = (RoomData)roomGrid[(curr.x+dx, curr.y+dy)];
        if (curr.rank < next.rank)
        {
            return (curr.rank + 2 * next.branchLength + 1 >= next.rank);
        }
        return (next.rank + 2 * curr.branchLength + 1 >= curr.rank);
    }
}
