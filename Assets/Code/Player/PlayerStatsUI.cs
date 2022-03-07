using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Text health;
    public Text sanity;
    public Text stamina;
    public Text hunger;
    public Text thirst;
    public Text resistance;

    public float updateRatio;


    private void Start()
    {
        InvokeRepeating("UpdateStats", 0, updateRatio);
    }

    void UpdateStats()
    {
        sanity.text     = "Sanity: "     + (int)PlayerStats.sanity;
        health.text     = "Health: "     + (int)PlayerStats.health;
        stamina.text    = "Stamina: "    + (int)PlayerStats.stamina;
        thirst.text     = "Thirst: "     + (int)PlayerStats.thirst;
        hunger.text     = "Hunger: "     + (int)PlayerStats.hunger;
        resistance.text = "Resistance: " + (int)PlayerStats.resistance;
    }
}
