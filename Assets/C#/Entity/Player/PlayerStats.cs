using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : EntityStats
{
    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
