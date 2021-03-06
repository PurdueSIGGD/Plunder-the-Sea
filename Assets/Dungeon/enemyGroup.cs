using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_Group", menuName = "ScriptableObjects/Generation/enemyGroup", order = 2)]
public class enemyGroup : ScriptableObject
{
    public GameObject[] enemySet;
}