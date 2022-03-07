using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerConditionalStatsHandler : MonoBehaviour
{
    PlayerStatsTimerHandler timerHandler;

    private void Start()
    {
        timerHandler = GetComponent<PlayerStatsTimerHandler>();
    }

    void ReduceStaminaWhileRunning()
    {
        if(Input.GetKeyDown(GameObjectRefs.player.GetComponent<vThirdPersonInput>().sprintInput))
        {
            timerHandler.ConsumeStaminaOverTime();
        }
        else if(Input.GetKeyUp(GameObjectRefs.player.GetComponent<vThirdPersonInput>().sprintInput))
        {
            timerHandler.StopConsumingStamina();
        }
    }

    private void Update()
    {
        ReduceStaminaWhileRunning();
    }


}
