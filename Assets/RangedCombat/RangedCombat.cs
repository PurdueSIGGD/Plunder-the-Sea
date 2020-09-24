using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : MonoBehaviour
{
    public GameObject projectilePrefab;
 
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            
        }
    }
}
