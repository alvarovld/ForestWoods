using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ProceduralObjectData : ScriptableObject
{
    public float radius;

    public GameObject prefab;
    public float heightFix;

    public float minScale;
    public float maxScale;

    public int maxAmount;

    public NoiseData noiseData;
    [Range(0,1)]
    public float cutHeight;

    public float gridVariability;

}
