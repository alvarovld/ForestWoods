using UnityEngine;
using System.Collections;
using Utils;

public class BluePrint : MonoBehaviour
{
    public float offset;

    public void SetOffset(float offset)
    {
        this.offset = offset;
    }

    void SetPosition()
    {
        var player = GameObjectRefs.player;
        gameObject.transform.position = TerrainHelper.AdjustPositionToFloor(player.position + player.forward * offset, 1);
    }

    void SetRotation()
    {
        gameObject.transform.rotation = GameObjectRefs.player.rotation;
    }

    private void Update()
    {
        SetPosition();
        SetRotation();
    }
}

