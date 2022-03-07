using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StickSpawn
{
    static Vector3 GetRandomPosition(Vector3 treePos, float floorOffset, float minDistance, float maxDistance)
    {
        int angle = Random.Range(0, 360);
        float distance = Random.Range(minDistance, maxDistance);
        var direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
        Vector3 position = treePos + direction.normalized * distance;
        var finalPos = Utils.TerrainHelper.AdjustPositionToFloor(position, floorOffset);
        if(finalPos.y < GameData.TerrainData.groundLevel)
        {
            finalPos = GetRandomPosition(treePos, floorOffset, minDistance, maxDistance);
        }
        return finalPos;
    }


    static Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    static int GetAmount(int maxAmount)
    {
        return Random.Range(0, maxAmount);
    }

    public static List<GameObject> Spawn(float minDistance,     
                                         float maxDistance, 
                                         int amount, 
                                         float floorOffset, 
                                         Vector3 treePos)
    {
        var stick = Resources.Load<GameObject>("Prefabs/Stick");
        var list = new List<GameObject>();
        for (int i = 0; i < amount; ++i)
        {
            stick.transform.position = GetRandomPosition(treePos, floorOffset, minDistance, maxDistance);
            stick.transform.rotation = GetRandomRotation();
            list.Add(ObjectPoolManager.GetInstance().GetObjectFromPool(stick.tag));
        }
        return list;
    }

    public static List<GameObject> Spawn(float minDistance, 
                                         float maxDistance,     
                                         int maxAmountPerTree,  
                                         int amount,
                                         float floorOffset,     
                                         List<Vector3> treePos)
    {
        List<GameObject> sticks = new List<GameObject>();
        foreach(var tree in treePos)
        {
            var amountPerTree = GetAmount(maxAmountPerTree);
            amount -= amountPerTree;
            if(amount <= 0)
            {
                break;
            }

            var st = Spawn(minDistance, maxDistance, amountPerTree, floorOffset, tree);
            sticks = sticks.Concat(st).ToList();
        }
        return sticks;
    }


}
