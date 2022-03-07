using UnityEngine;
using System.Collections.Generic;
using Utils;
using System.Linq;
using System;

public class ItemSpawnManager : MonoBehaviour
{
    [Serializable]
    public class PrefabAmount
    {
        public GameObject prefab;
        public int amount;
    }

    Dictionary<string, List<GameObject>> objectReferenceDic = new Dictionary<string, List<GameObject>>();
    Vector3 playerPosBeforeSpawn;

    // Inspector paramenters

    [Header("General items")]
    public List<PrefabAmount> prefabAmountList;
    public float floorOffset;
    public float visionDistance;
    public float maxRadius;
    public float triggerSpawnDistance;

    [Header("Sticks")]
    public int maxAmountPerTree;
    public float minStickDistance;
    public float maxStickDistance;


    GameObject treeParent;


    private void Start()
    {
        treeParent = GameObject.Find("TreeParent");
        playerPosBeforeSpawn = GameObjectRefs.player.position;

        Spawn(30);
        InvokeRepeating("SpawnWhenOffLimit", 0, 1f);
    }

    List<Vector3> GetTreesInSpawnRange(float minDistance)
    {
        List<Vector3> treesInSpawnRange = new List<Vector3>();
        for(int i = 0; i < treeParent.transform.childCount; ++i)
        {
            var childPos = treeParent.transform.GetChild(i).position;
            if(IsPositionInSpawnRange(childPos, minDistance))
            {
                treesInSpawnRange.Add(childPos);
            }
        }
        return treesInSpawnRange;
    }

    bool IsPositionInSpawnRange(Vector3 pos, float minDistance)
    {
        var distanceToPlayer = (GameObjectRefs.player.position - pos).magnitude;
        return distanceToPlayer < maxRadius &&
               distanceToPlayer > minDistance;
    }

    List<GameObject> DestroyNotVisible(List<GameObject> list)
    {
        foreach (GameObject obj in list.Reverse<GameObject>())
        {
            if(!obj)
            {
                continue;
            }
            if ((obj.transform.position - GameObjectRefs.player.position).magnitude <= visionDistance)
            {
                continue;
            }
            ObjectPoolManager.GetInstance().ReturnObjectToPoolDisabling(obj);
            list.Remove(obj);
        }
        return list;
    }

    void Spawn(float visionDistance)
    {
        foreach(PrefabAmount objectAmount in prefabAmountList)
        {
            int fixedAmount;
            string key = objectAmount.prefab.tag;
            if (objectReferenceDic.ContainsKey(key))
            { 
                var objectsLeft = DestroyNotVisible(objectReferenceDic[key]);
                fixedAmount = objectAmount.amount - objectsLeft.Count;
                objectReferenceDic.Remove(key);
                objectReferenceDic.Add(key, objectsLeft);
            }
            else
            {
                fixedAmount = objectAmount.amount;
            }

            if(fixedAmount.Equals(0))
            {
                continue;
            }

            var references = objectAmount.prefab.tag.Equals(GameData.Tags.Stick) ?
                             StickSpawn.Spawn(minStickDistance, maxStickDistance, maxAmountPerTree,
                                              objectAmount.amount, floorOffset, GetTreesInSpawnRange(visionDistance)) :
                             ItemSpawn.Spawn(objectAmount.prefab, fixedAmount, floorOffset, visionDistance, maxRadius);
            
            if (objectReferenceDic.ContainsKey(key))
            {
                // add new references to objects left in list
                objectReferenceDic[key] = objectReferenceDic[key].Concat(references).ToList();
            }
            else
            {
                objectReferenceDic.Add(key, references);
            }
        }
    }

    void SpawnWhenOffLimit()
    {
        if (HasToTriggerSpawn())
        {
            Spawn(visionDistance);
            playerPosBeforeSpawn = GameObjectRefs.player.position;
        }
    }


    bool HasToTriggerSpawn()
    {
        var dir = (GameObjectRefs.player.position - playerPosBeforeSpawn).normalized;
        var limitPoint = playerPosBeforeSpawn + dir * maxRadius;
        var distanceToLimit = (GameObjectRefs.player.position - limitPoint).magnitude;

        return distanceToLimit <= triggerSpawnDistance;
    }

}
