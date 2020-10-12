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
    public int truePathLength;

    private Hashtable roomGrid;
    private Stack<RoomData> roomStack;

    public class RoomData
    {
        public int x;
        public int y;
        public List<int> nextDirs;
        public bool[] connectDirs;
        public RoomData parent;

        public RoomData(int x, int y, RoomData p)
        {
            this.x = x;
            this.y = y;
            nextDirs = new List<int>(new int[] {0 ,1, 2, 3});
            parent = p;
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
        RoomData curr = new RoomData(0, 0, null);
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
                RoomData nextRoom = new RoomData(x+dx, y+dy, curr);
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

        while (roomStack.Count > 0)
        {
            buildRoom(roomStack.Pop());
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
                    Debug.Log("Hall: " + hallLoc);
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
        Debug.Log("Room: " + (newRoom.x,newRoom.y));
        Object.Instantiate(room, new Vector3(newRoom.x * roomScale, newRoom.y * roomScale, 0), Quaternion.identity);
    }

    public bool validNext(int x, int y, int dx, int dy)
    {
        return !roomGrid.Contains((x + dx, y + dy));
    }
}
