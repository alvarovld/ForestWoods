using UnityEngine;
using System.Collections;
using ItemEnum = GameData.Enums.Items;
using Utils;

public static class PlayerStats 
{
    public static float health = 100;
    public static float stamina = 100;
    public static float resistance = 100;
    public static float sanity = 100;
    public static float hunger = 100;
    public static float thirst = 100;
    public static float strength = 20;

    public static bool sleepAvailable = true;
    public static float timeBetweenSleeps = 10;
    private static PlayerSleepTime sleepTimer;

    public static void ReduceHealth(float h)
    {
        health -= h;
    }

    public static bool Sleep()
    {
        if(sleepTimer == null)
        {
            sleepTimer = GameObjectRefs.player.gameObject.AddComponent<PlayerSleepTime>();
        }

        if(!sleepAvailable)
        {
            return false;
        }
        
        resistance = 100;
        sleepTimer.Sleep();
        return true;
    }


    static void RestoreHunger(float amount)
    {
        hunger += amount;
        if (hunger > 100)
        {
            hunger = 100;
        }
    }

    public static void Feed(ItemEnum item)
    {
        switch (item)
        {
            case ItemEnum.CookedFish:
                RestoreHunger(30);
                break;
            case ItemEnum.Blueberry:
                RestoreHunger(10);
                break;
            default:
                Debug.LogError("[PlayerStats] Unknown food: " + item);
                return;
        }
    }
}
