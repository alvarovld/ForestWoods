using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookConverter : MonoBehaviour
{
    string tagBeforeCook;
    public string tagAfterCook;

    public void ConvertInCookedItem()
    {
        gameObject.tag = tagAfterCook;
    }
}
