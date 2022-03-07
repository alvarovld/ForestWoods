using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustToFloor : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitThenExecute());
    }

    IEnumerator WaitThenExecute()
    {
        yield return new WaitForSeconds(2f);
        transform.position = Utils.TerrainHelper.AdjustPositionToFloor(transform.position);

    }
}
