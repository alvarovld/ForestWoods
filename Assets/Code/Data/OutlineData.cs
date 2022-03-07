using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OutlineData : ScriptableObject
{
    public float distanceToShine;
    [Range(0, 1)]
    public int defaultColor;
}
