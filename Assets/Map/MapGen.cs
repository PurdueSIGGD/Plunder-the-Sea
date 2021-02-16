﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update

    public const int ROOMWIDTH = 10;
    public const int ROOMDIST = 6;
    public GameObject[] rooms;
    public GameObject[] halls;
    public GameObject wall;
    public GameObject goal;
    public Sprite[] floors;
    public Sprite[] walls;
    private Sprite chosenFloor;
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
        chosenFloor = floors[UnityEngine.Random.Range(0, floors.Length)];
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
            // check if cornered
            if (curr.nextDirs.Count <= 0)
            {
                RoomData deadEnd = curr;
                roomStack.Pop();
                roomGrid.Remove((x, y));
                curr = roomStack.Peek();
                x = curr.x;
                y = curr.y;
                length--;
                int oldDir = toDirCode(deadEnd.x - x, deadEnd.y - y);
                curr.connectDirs[oldDir] = false;

                continue;
            }
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
        Object.Instantiate(goal, new Vector3(lastRoom.x * roomScale,
            lastRoom.y * roomScale, 0), Quaternion.identity);
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
                    int nx = curr.x + dirs.Item1;
                    int ny = curr.y + dirs.Item2;
                    if (connectNext(curr, dirs.Item1, dirs.Item2) && UnityEngine.Random.Range(0, 1f) <= branchFactor)
                    {
                        //create branch in this direction
                        RoomData nextRoom;
                        
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
                    else if (roomGrid.Contains((nx, ny)))
                    {
                        //If there is a room in this direction, make sure not to check this hallway spot
                        //when processing that room
                        RoomData rejectRoom = (RoomData)roomGrid[(nx, ny)];
                        rejectRoom.nextDirs.Remove((curr.nextDirs[i] + 2) % 4);
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
    
    public int toDirCode(int dx, int dy)
    {
        int code = 0;
        if (dx + dy < 0)
        {
            code += 2;
        }
        code += math.abs(dx);
        return code;
    }

    public Sprite getWall()
    {
        if (UnityEngine.Random.Range(0, 1f) < 0.5f)
        {
            return walls[0];
        } 
        else
        {
            return walls[UnityEngine.Random.Range(0, walls.Length)];
        }
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
                        rot = Quaternion.Euler(0, 0, 180 * UnityEngine.Random.Range(0, 2));
                    }
                    else
                    {
                        rot = Quaternion.Euler(0, 0, 90 + 180 * UnityEngine.Random.Range(0, 2));
                    }
                    GameObject g2 = Instantiate(halls[UnityEngine.Random.Range(0, halls.Length)], new Vector3(hallLoc.Item1 * roomScale,
                        hallLoc.Item2 * roomScale, 0), rot);
                    foreach (SpriteRenderer SR in g2.GetComponentsInChildren<SpriteRenderer>())
                    {
                        SR.transform.rotation = Quaternion.Euler(Vector3.zero);
                        if (SR.name.Contains("Wall"))
                        {
                            SR.sprite = getWall();
                        }
                        else
                        {
                            SR.sprite = chosenFloor;
                        }
                    }
                }
            }
            else
            {
                //plug opening
                (float, float) wallLoc = (newRoom.x * roomScale + .5f * ((dir.Item1) * ROOMWIDTH - dir.Item1),
                    newRoom.y * roomScale + .5f * ((dir.Item2) * ROOMWIDTH - dir.Item2));
                Quaternion rot;
                if (dir.Item2 == 0)
                {
                    rot = Quaternion.identity;
                }
                else
                {
                    rot = Quaternion.Euler(0, 0, 90);
                }
                GameObject g = Instantiate(wall, new Vector3(wallLoc.Item1, wallLoc.Item2, 0), rot);
                foreach (SpriteRenderer SR in g.GetComponentsInChildren<SpriteRenderer>())
                {
                    SR.transform.rotation = Quaternion.Euler(Vector3.zero);
                    SR.sprite = getWall();
                }
            }
        }
        //Debug.Log("Room: " + (newRoom.x,newRoom.y) + ", " + (newRoom.rank, newRoom.branchLength));

        //Spawns rooms
        int randomRotation = UnityEngine.Random.Range(0, 4);
        GameObject g3 = Object.Instantiate(rooms[UnityEngine.Random.Range(0, rooms.Length)], new Vector3(newRoom.x * roomScale, newRoom.y * roomScale, 0), Quaternion.Euler(0, 0, randomRotation*90));
        foreach (SpriteRenderer SR in g3.GetComponentsInChildren<SpriteRenderer>())
        {
            SR.transform.rotation = Quaternion.Euler(Vector3.zero);
            if (SR.name.Contains("Wall"))
            {
                SR.sprite = getWall();
            }
            else
            {
                SR.sprite = chosenFloor;
            }
        }
        //spawns enemies
        if (newRoom.x != 0 || newRoom.y != 0)
        {
            foreach (EnemySpawner ES in g3.GetComponentsInChildren<EnemySpawner>()) { 
                ES.spawnEnemies();
            }
        }
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
