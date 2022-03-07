using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class PlayerSleepTime : MonoBehaviour
{
    public void Sleep()
    {
        StartCoroutine(NextSleepTimer());
    }

    static IEnumerator NextSleepTimer()
    {
        PlayerStats.sleepAvailable = false;
        yield return new WaitForSeconds(PlayerStats.timeBetweenSleeps);
        PlayerStats.sleepAvailable = true;
    }
}
