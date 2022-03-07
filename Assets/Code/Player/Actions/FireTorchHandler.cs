using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTorchHandler : MonoBehaviour
{
    bool firstTurnedOn;
    bool extincted;

    private void Start()
    {
        firstTurnedOn = false;
        extincted = false;
    }
    public void TurnOn()
    {
        if(extincted)
        {
            return;
        }
        if(!firstTurnedOn)
        {
            firstTurnedOn = true;
            StartCoroutine(TurnOffWhenExtincted());
        }

        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator TurnOffWhenExtincted()
    {
        yield return new WaitForSeconds(20);
        transform.GetChild(0).gameObject.SetActive(false);
        extincted = true;
    }
}
