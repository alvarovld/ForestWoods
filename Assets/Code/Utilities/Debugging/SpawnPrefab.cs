using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{

    public GameObject prefab;
    public int quantity;
    void Start()
    {
        GameObject prefabs = GameObject.Find("Spawn");
        prefabs.name = prefabs.name + " " + prefab.name + " (Clones)";

        for (int i = 0; i < quantity; ++i)
        {
            GameObject clone = Instantiate(prefab);
            clone.transform.SetParent(prefabs.transform);
        }
    }
}
