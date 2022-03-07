using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanLiquid : MonoBehaviour
{
    private void OnEnable()
    {
        SetTagState();
        StartCoroutine(ChangeTag());
    }

    void SetTagState()
    {
        if (transform.GetChild(2).gameObject.activeSelf)
        {
            gameObject.tag = GameData.Tags.FilledCan;
        }
        else
        {
            gameObject.tag = GameData.Tags.EmptyCan;
        }
    }

    IEnumerator ChangeTag()
    {
        bool state = transform.GetChild(2).gameObject.activeSelf;
        yield return new WaitUntil(() => { return transform.GetChild(2).gameObject.activeSelf != state; });
        SetTagState();
        StartCoroutine(ChangeTag());
    }

    public void Fill()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        gameObject.tag = GameData.Tags.FilledCan;
    }

    public void Empty()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.tag = GameData.Tags.EmptyCan;
    }
}
