using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : EntityStats
{

    PlayerBase pbase;

    private void Start()
    {
        pbase = GetComponent<PlayerBase>();
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void OnKill(EntityStats victim)
    {
        pbase.OnKill(victim);
    }

}
