using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_Preset", menuName = "ScriptableObjects/Generation/presetData", order = 1)]
public class presetData : ScriptableObject
{

    public Sprite mainWall;
    public Sprite[] secondaryWalls;
    public Sprite floor;
    public Sprite[] uniques;
    public Sprite[] voidImages;
    public GameObject[] rooms;
    public GameObject[] halls;
    public GameObject wall;
    public int roomWidth = 10;
    public int roomDist = 6;
}
