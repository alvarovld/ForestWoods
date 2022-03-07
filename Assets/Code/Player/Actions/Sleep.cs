using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Sleep : MonoBehaviour
{
    ActionSwitcher actionSwitcher;

    private void Start()
    {
        actionSwitcher = gameObject.AddComponent<ActionSwitcher>();
        actionSwitcher.SetPositionProperties(transform, new Vector3(0, 3, 0));
    }   



    private void OnTriggerStay(Collider other)
    {
        if (!PlayerStats.sleepAvailable)
        {
            actionSwitcher.RemoveAllActions();
            return;
        }
        if (!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }
        if (GameObjectRefs.player.GetComponent<Rigidbody>().velocity.magnitude > 1)
        {
            return;
        }

        actionSwitcher.AddActionIfNotExist("Sleep", SleepAction);
        actionSwitcher.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        actionSwitcher.enabled = false;
    }

    void SleepAction()
    {
        // Pass callback as lambda parameter
        GameObjectRefs.camera.GetComponent<FadeCamera>().Fade(() =>
        {
            GameObjectRefs.player.GetComponent<vThirdPersonInput>().enabled = true;
            PlayerStats.Sleep();
        });

        GameObjectRefs.player.GetComponent<vThirdPersonInput>().enabled = false;

    }
}
