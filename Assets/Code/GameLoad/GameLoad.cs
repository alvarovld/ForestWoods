using System;
using UnityEngine;
using Utils;

public class GameLoad : MonoBehaviour
{
    void Start()
    {
        EndlessTerrain.GetInstance().InitialMapLoaded += OnMapLoaded;
        GameObjectRefs.camera.enabled = false;
        CraftableItems.InitializeData();
    }

    void OnMapLoaded(object sender, EventArgs args)
    {
        GameObjectRefs.player.position = TerrainHelper.AdjustPositionToFloor(GameObjectRefs.player.position, 6);
        GameObjectRefs.camera.enabled = true;
    }
}
