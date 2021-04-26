using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private presetData[] presets;

    private int roomWidth = 10;
    private int roomDist = 6;
    private GameObject[] rooms;
    private GameObject[] halls;
    private GameObject wall;
    private Sprite floor;
    private Sprite mainWall;
    private Sprite[] secondaryWalls;
    private Sprite[] uniqueObjects;
    private Sprite[] voidObjects;
    [SerializeField]
    private GameObject voidPrefab;
    [SerializeField]
    private GameObject chestPrefab;

    public GameObject goal;
    public GameObject boss;

    private int truePathLength;
    private int maxBranchLength;
    private float branchFactor;

    private Hashtable roomGrid;
    private Stack<RoomData> roomStack;
    private int weaponsCount;

    public bool bossLevel = false;

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

        //dungeon scaling
        int level = 0;
        foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
        {
            level = Mathf.Max(level, ps.dungeonLevel);
        }
        int presetNum = level / 2;
        if (presetNum > presets.Length - 1)
        {
            presetNum = UnityEngine.Random.Range(0, presets.Length);
        }
        assignPresetData(presets[presetNum]);
        truePathLength = 3 + (int) Mathf.Min(level * 0.5f, 5);
        maxBranchLength = 1 + (int) Mathf.Min(level * 0.1f, 1);
        branchFactor = 0.1f + Mathf.Min(level * 0.05f, 0.4f);

        weaponsCount = System.Enum.GetValues(typeof(WeaponFactory.CLASS)).Length;

        generate();
    }

    private void assignPresetData(presetData PD)
    {
        roomWidth = PD.roomWidth;
        roomDist = PD.roomDist;
        rooms = PD.rooms;
        halls = PD.halls;
        wall = PD.wall;
        floor = PD.floor;
        mainWall = PD.mainWall;
        secondaryWalls = PD.secondaryWalls;
        uniqueObjects = PD.uniques;
        voidObjects = PD.voidImages;
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
            RoomData room = roomStack.Pop();
            buildRoom(room);
            makeChest(room);
        }

        //This is a janky implementation of placing a change scene door in the final room
        //Changes to the generation algorithm could break this and it is not very robust
        //Handling room spawns and customization will have to be implemented later
        RoomData lastRoom = roomStack.Pop();
        buildRoom(lastRoom);
        int roomScale = roomWidth + roomDist;
        Vector3 goalPos = new Vector3(lastRoom.x * roomScale, lastRoom.y * roomScale, 0);
        // Based on level, decide between boss or normal goal
        int level = 0;
        foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
        {
            level = Mathf.Max(level, ps.dungeonLevel);
        }
        if (level >= 9 && (level-9)%5 == 0)
        {
            // Spawn King Crab
            Object.Instantiate(boss, goalPos, Quaternion.identity);
            bossLevel = true;
        } else
        {
            // Spawn gate
            Object.Instantiate(goal, goalPos, Quaternion.identity);
        }
        

        //Sets sprites

        List<SpriteRenderer> toDupe = new List<SpriteRenderer>();
        List<SpriteRenderer> toMove = new List<SpriteRenderer>();

        //this finds every vertical wall
        foreach (SpriteRenderer SR in FindObjectsOfType<SpriteRenderer>())
        {
            //if wall
            if (SR.name.Contains("Wall"))
            {
                //loop to check if above vertical
                foreach (SpriteRenderer SR2 in FindObjectsOfType<SpriteRenderer>())
                {
                    if (SR2.name.Contains("Wall"))
                    {
                        //if true -> vertical (not top one)
                        if (Vector3.Distance(SR2.transform.position, SR.transform.position - Vector3.up) < 0.1f)
                        {
                            toDupe.Add(SR);
                            toMove.Add(SR);
                            break;
                        }
                        //if true -> vertical (not bottem one)
                        if (Vector3.Distance(SR2.transform.position, SR.transform.position + Vector3.up) < 0.1f)
                        {
                            toMove.Add(SR);
                        }
                    }
                }
            }
        }

        foreach (SpriteRenderer SR in toDupe)
        {
            Instantiate(SR.gameObject, SR.transform.position - Vector3.up * 0.5f, SR.transform.rotation);
        }

        foreach (SpriteRenderer SR in toMove)
        {
            SR.transform.position -= Vector3.forward;
        }

        //adds a few void object for each grid area
        int gridScale = roomDist + roomWidth;
        int maxSize = truePathLength + maxBranchLength;
        for (int i = -maxSize; i <= maxSize; i++)
        {
            for (int j = -maxSize; j <= maxSize; j++)
            {
                for (int k = 0; k < gridScale; k++)
                {
                    GameObject g = Instantiate(voidPrefab, new Vector3(i * gridScale + UnityEngine.Random.Range(0, gridScale) - 0.5f, j * gridScale + UnityEngine.Random.Range(0, gridScale) - 0.5f, 2), Quaternion.identity);
                    g.GetComponent<SpriteRenderer>().sprite = voidObjects[UnityEngine.Random.Range(0, voidObjects.Length)];
                }
            }
        }

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
        if (UnityEngine.Random.Range(0, 1f) < 0.75f)
        {
            return mainWall;
        } 
        else
        {
            return secondaryWalls[UnityEngine.Random.Range(0, secondaryWalls.Length)];
        }
    }

    public void getUnique(SpriteRenderer SR)
    {
        if (UnityEngine.Random.Range(0, 1f) < 0.75f)
        {
            SR.sprite = null;
            Destroy(SR.gameObject, 0.5f);
        }
        else
        {
            SR.sprite =  uniqueObjects[UnityEngine.Random.Range(0, uniqueObjects.Length)];
        }
    }

    public void makeChest(RoomData newRoom)
    {
        //spawns chest
        if (UnityEngine.Random.Range(0, 1f) < 0.2f && (newRoom.x != 0 || newRoom.y != 0))
        {
            WeaponFactory.CLASS wepClass = (WeaponFactory.CLASS)UnityEngine.Random.Range(0, weaponsCount);
            //bottle check
            if (wepClass == WeaponFactory.CLASS.BOTTLE)
            {
                return;
            }

            int roomScale = roomWidth + roomDist;
            GameObject chest = Instantiate(chestPrefab, new Vector3(newRoom.x * roomScale, newRoom.y * roomScale, -1), Quaternion.identity);
            chest.GetComponent<ChestBehaviour>().SetWeaponClass(wepClass);
        }
    }

    public void buildRoom(RoomData newRoom)
    {
        int roomScale = roomWidth + roomDist;
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
                            cornerWallFix CWF;
                            if (CWF = SR.GetComponent<cornerWallFix>())
                            {
                                CWF.fixCorner();
                            }
                        }
                        if (SR.name.Contains("Floor"))
                        {
                            SR.sprite = floor;
                        }
                        if (SR.name.Contains("Unique"))
                        {
                            getUnique(SR);
                        }
                    }
                }
            }
            else
            {
                //plug opening
                (float, float) wallLoc = (newRoom.x * roomScale + .5f * ((dir.Item1) * roomWidth - dir.Item1),
                    newRoom.y * roomScale + .5f * ((dir.Item2) * roomWidth - dir.Item2));
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
                cornerWallFix CWF;
                if (CWF = SR.GetComponent<cornerWallFix>())
                {
                    CWF.fixCorner();
                }
            }
            if (SR.name.Contains("Floor"))
            {
                SR.sprite = floor;
            }
            if (SR.name.Contains("Unique"))
            {
                getUnique(SR);
            }
        }

        //if not fist room
        if (newRoom.x != 0 || newRoom.y != 0)
        {
            //spawns enemies
            g3.GetComponent<EnemySpawner>().spawnEnemies();
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
