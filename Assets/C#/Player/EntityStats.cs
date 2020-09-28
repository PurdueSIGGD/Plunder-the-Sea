using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EntityStats : MonoBehaviour
{

    public float movementSpeed = 10.0f;
    public float maxHP = 1;
    public float currentHP = 1;

    public void TakeDamage(float amount)
    {

        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

}
