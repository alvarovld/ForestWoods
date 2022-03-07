using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    Transform fire;
    public ActionSwitcher switcher;
    public int timeToPutOut;
    public string PUT_OUT = "Put out";

    private void Awake()
    {
        switcher = gameObject.AddComponent<ActionSwitcher>();
        switcher.SetPositionProperties(transform, GameData.Parameters.campFireTextOffset);
    }
    void Start()
    {
        fire = transform.GetChild(0);
        StartCoroutine(PutOutFire());
    }

    public void PutOut()
    {
        fire.gameObject.SetActive(false);
        switcher.RemoveActionIfExists(PUT_OUT);
        gameObject.GetComponent<CookManager>().AddPickUpActionToItemInCampfireIfExist();
    }

    public bool IsFireActive()
    {
        return fire.gameObject.activeSelf;
    }

    IEnumerator PutOutFire()
    {
        yield return new WaitForSeconds(timeToPutOut);
        fire.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }
        switcher.enabled = true;
        if (fire.gameObject.activeSelf)
        {
            switcher.AddActionIfNotExist(PUT_OUT, PutOut);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }

        switcher.enabled = false;
    }
}
