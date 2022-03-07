using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class ItemSpawn
{
    public static List<GameObject> Spawn(GameObject prefab, 
                                  int amount, 
                                  float floorOffset, 
                                  float minRadius, 
                                  float maxRadius)
    {
        List<GameObject> items = new List<GameObject>();

        for (int i = 0; i < amount; ++i)
        {
            prefab.transform.position = GetRandomPosition(minRadius,maxRadius,floorOffset);
            prefab.transform.rotation = GetRandomRotation();
            var clone = ObjectPoolManager.GetInstance().GetObjectFromPool(prefab.tag);
            if(clone == null)
            {
                return null;
            }
            items.Add(clone);
        }
        return items;
    }

    static Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0,360), 0);
    }
    static Vector3 GetRandomPosition(float minRadius, float maxRadius, float floorOffset)
    {
        float randomDistance = Random.Range(minRadius, maxRadius);
        float playerAngle;
        Vector3 playerAxis;
        GameObjectRefs.player.rotation.ToAngleAxis(out playerAngle, out playerAxis);
        int angleOffset = Random.Range(0, 360);
        var direction = Quaternion.AngleAxis(playerAngle + angleOffset, Vector3.up) * Vector3.forward;
        var position = GameObjectRefs.player.position + direction.normalized * randomDistance;
        var finalPos = TerrainHelper.AdjustPositionToFloor(position, floorOffset);

        if(finalPos.y < GameData.TerrainData.groundLevel)
        {
            finalPos = GetRandomPosition(minRadius, maxRadius, floorOffset);
        }
        return finalPos;
    }

}
