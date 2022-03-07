using System.Collections;
using UnityEngine;

public class PlayerStatsTimerHandler : MonoBehaviour
{
    [Header("General")]
    public float timeBetweenCounts;

    [Tooltip("The percentage the stat gets reduced every second")]
    [Header("Ratios")]
    public float hungerRatio;
    public float thirstRatio;
    public float resistanceRatio;
    public float staminaReduceRatio;
    public float staminaIncreaseRatio;
    public float tiredTime;
    public float hungerThirstInfluenceInResistance;

    bool isConsumingStamina;
    bool isRestoringStamina;

    
    private void OnEnable()
    {
        isConsumingStamina = false;

        StartCoroutine(IncreaseHunger());
        StartCoroutine(IncreaseThirst());
        StartCoroutine(ConsumeResistance());
    }

    public void ConsumeStaminaOverTime()
    {
        isConsumingStamina = true;
        StartCoroutine(ConsumeStamina());
    }

    IEnumerator RestoreStamina()
    {
        isRestoringStamina = true;

        if (PlayerStats.stamina <= 0)
        {
            StartCoroutine(PlayerTired());
            yield break;
        }
        yield return new WaitForSeconds(timeBetweenCounts);
        PlayerStats.stamina = PlayerStats.stamina + staminaIncreaseRatio * timeBetweenCounts;
        
        if(PlayerStats.stamina >= PlayerStats.resistance)
        {
            PlayerStats.stamina = PlayerStats.resistance;
            isRestoringStamina = false;
            yield break;
        } else if(isConsumingStamina)
        {
            isRestoringStamina = false;
            yield break;
        }

        StartCoroutine(RestoreStamina());
    }

    IEnumerator PlayerTired()
    {
        yield return new WaitForSeconds(tiredTime);
        PlayerStats.stamina = 1;
        StartCoroutine(RestoreStamina());

    }

    IEnumerator ConsumeResistance()
    {
        yield return new WaitForSeconds(timeBetweenCounts);
        //var hungerThirstRatio = ((PlayerStats.hunger + PlayerStats.thirst) / 200) / hungerThirstInfluenceInResistance;

        PlayerStats.resistance = PlayerStats.resistance - resistanceRatio * timeBetweenCounts; //* hungerThirstRatio;
        
        if(!isConsumingStamina && !isRestoringStamina)
        {
            PlayerStats.stamina = PlayerStats.resistance;
        }

        if (PlayerStats.resistance > 0)
        {
            StartCoroutine(ConsumeResistance());
        }
        else if(PlayerStats.resistance < 0)
        {
            PlayerStats.resistance = 0;
        }
    }

    IEnumerator ConsumeStamina()
    {
        if(PlayerStats.stamina <= 0)
        {
            PlayerStats.stamina = 0;
            yield break;
        }

        yield return new WaitForSeconds(timeBetweenCounts);
        PlayerStats.stamina = PlayerStats.stamina - staminaReduceRatio * timeBetweenCounts;

        if (!isConsumingStamina)
        {
            yield break;
        }

        StartCoroutine(ConsumeStamina());
    }

    public void StopConsumingStamina()
    {
        isConsumingStamina = false;
        StartCoroutine(RestoreStamina());
    }




    IEnumerator IncreaseHunger()
    {
        PlayerStats.hunger = PlayerStats.hunger - hungerRatio * timeBetweenCounts;
        yield return new WaitForSeconds(timeBetweenCounts);
        if (PlayerStats.hunger > 0)
        {
            StartCoroutine(IncreaseHunger());
        }
        else if(PlayerStats.hunger < 0)
        {
            PlayerStats.hunger = 0;
        }
    }
    IEnumerator IncreaseThirst()
    {
        PlayerStats.thirst = PlayerStats.thirst - thirstRatio * timeBetweenCounts;
        yield return new WaitForSeconds(timeBetweenCounts);
        if (PlayerStats.thirst > 0)
        {
            StartCoroutine(IncreaseThirst());
        }
        else if(PlayerStats.thirst < 0)
        {
            PlayerStats.thirst = 0;
        }
    }




}
