using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update

    public const int ROOMWIDTH = 10;
    public const int ROOMDIST = 6;
    public GameObject room;
    public int truePathLength;

    private Hashtable roomGrid;
    private Stack<roomData> roomStack;

    public struct roomData
    {
        public int x;
        public int y;

        public roomData(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    void Start()
    {
        roomGrid = new Hashtable();
        roomStack = new Stack<roomData>();
        generate();
    }

    // Update is called once per frame
    public void generate()
    {
        int length = 0;
        int x = 0;
        int y = 0;
        while (length < truePathLength)
        {
            int direction = Random.Range(0, 4);
            int dx = 0;
            int dy = 0;
            if (direction % 2 == 0)
            {
                dx = 1;
            }
            else
            {
                dy = 1;
            }
            if (direction > 1)
            {
                dx *= -1;
                dy *= -1;
            }
            if (validNext(x, y, dx, dy))
            {
                roomData nextRoom = new roomData(x+dx, y+dy);
                roomGrid.Add((nextRoom.x, nextRoom.y), nextRoom);
                roomStack.Push(nextRoom);
                x = nextRoom.x;
                y = nextRoom.y;
                length++;
            }
        }

        while (roomStack.Count > 0)
        {
            buildRoom(roomStack.Pop());
        }
    }

    public void buildRoom(roomData newRoom)
    {
        int roomScale = ROOMWIDTH + ROOMDIST;
        Object.Instantiate(room, new Vector3(newRoom.x * roomScale, newRoom.y * roomScale, 0), Quaternion.identity);
    }

    public bool validNext(int x, int y, int dx, int dy)
    {
        if (roomGrid.Contains((x + dx, y + dy)))
        {
            return false;
        }
        return true;
    }
}
