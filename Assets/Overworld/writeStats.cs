﻿using UnityEngine;
using UnityEngine.UI;

public class writeStats : MonoBehaviour
{
    [SerializeField]
    private bool text1;
    [SerializeField]
    private bool text2;

    private void Start()
    {
        if (text1)
        {
            GetComponent<Text>().text = string.Format("Dungeon Level: {0}\nTime: {1} Seconds\nKills: {2}\nTotal damage dealt: {3}\nTotal damage taken: {4}\nDied to: {5}\nClass: {6}",
                PlayerPrefs.GetInt("Level"), ((int)(100*PlayerPrefs.GetFloat("Time")))/100f, PlayerPrefs.GetInt("Kills"), PlayerPrefs.GetInt("Damage"), PlayerPrefs.GetInt("Hurt"),
                PlayerPrefs.GetString("Killer"), PlayerPrefs.GetString("Class"));
        }
        else
        {
            if (text2)
            {
                GetComponent<Text>().text = string.Format("Total bait collected: {0}\nBait spent fishing: {1}\nFish caught: {2}\nBait spent on weapons: {3}\n" +
                    "Chests opened: {4}\nWeapons collected: {5}\nTotal bait remaining: {6}",
                    PlayerPrefs.GetInt("BaitGot"), PlayerPrefs.GetInt("FishingBait"), PlayerPrefs.GetInt("Caught"), PlayerPrefs.GetInt("WeaponBait"),
                    PlayerPrefs.GetInt("Chests"), PlayerPrefs.GetInt("Weapons"), PlayerPrefs.GetInt("BaitLeft"));
            }
            else
            {
                string tip = "Try again?";
                string playerClass = PlayerPrefs.GetString("Class");
                if (playerClass == "Test Class")
                {
                    tip = "The Test class is average in every way and has no special abilities, focus on dodgeing and attacking.";
                }
                if (playerClass == "Gunner")
                {
                    tip = "The Gunner class excels in ranges combat, but needs to use melee to gain ammo. Use guns to lower enemy's health then kill with melee to regain ammo.";
                }
                if (playerClass == "Brawler")
                {
                    tip = "The Brawler class is melee focused, make sure to dodge the shots of ranged opponents.";
                }
                if (playerClass == "First Mate")
                {
                    tip = "The First Mate must kill enemies quickly to activate the kill chain, try killing weaker enemies first.";
                }
                if (playerClass == "Swash buckler")
                {
                    tip = "The Swashbuckler can quickly strike twice while avoiding most damage by attacking an enemy with melee then with ranged.";
                }
                if (playerClass == "Warrant Officer")
                {
                    tip = "The Warrent Officer has a chance to create chain lighting, try grouping enemies together to take them out all at once.";
                }
                if (PlayerPrefs.GetFloat("Time") > Mathf.Pow(PlayerPrefs.GetFloat("Level") + 1, 1.25f) * 60)
                {
                    tip = "There is no need to do everything in a level, getting to the exit is your main goal.";
                }
                if (PlayerPrefs.GetInt("BaitLeft") > PlayerPrefs.GetInt("BaitGot")/2 && PlayerPrefs.GetInt("FishingBait") + PlayerPrefs.GetInt("WeaponBait") < PlayerPrefs.GetInt("Level")*2)
                {
                    tip = "Use the bait you collect from killing enemies to fish or buy new weapons.";
                }
                if (playerClass == "Captain")
                {
                    tip = "The Captain class only has one health, don't get hit and use your deadly weapons when there is an opening.";
                }
                if (PlayerPrefs.GetInt("Level") > 20)
                {
                    tip = "I have nothing left to tell you...";
                }
                GetComponent<Text>().text = "\"" + tip + "\"";
            }
        }
    }
}
