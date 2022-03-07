
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class EnemySpawn : MonoBehaviour
{
    List<GameObject> enemyPool = new List<GameObject>();
    public GameObject prefab;
    public float minDistance;
    public float maxDistance;
    public float distanceToDespawn;
    public int minTimeSpawn;
    public int maxTimeSpawn;
    public int maxAmount;

    [Header("Enemy Groups")]
    public float minDistanceInEnemyGroups;
    public float maxDistanceInEnemyGroups;
    public int maxAmountInAGroup;

    private void Start()
    {
        InvokeRepeating("Despawn", 20, 5);
        StartCoroutine(TriggerSpawn());
    }

    void Despawn()
    {
        foreach(var enemy in enemyPool.Reverse<GameObject>())
        {
            if((enemy.transform.position - GameObjectRefs.player.position).magnitude > distanceToDespawn)
            {
                ObjectPoolManager.GetInstance().ReturnObjectToPoolDisabling(enemy);
                enemyPool.Remove(enemy);
            }
        }
    }

    IEnumerator TriggerSpawn()
    {
        yield return new WaitForSeconds(GetRandomTime());
        Spawn(GetRandomAmount());
        StartCoroutine(TriggerSpawn());
    }

    float GetRandomTime()
    {
        return Random.Range(minTimeSpawn, maxTimeSpawn);
    }

    int GetRandomAmount()
    {
        return Random.Range(1, 3);
    }

    float GetRandomDistance(float minDistance, float maxDistance)
    {
        return Random.Range(minDistance, maxDistance);
    }

    Vector3 GetRandomDirection()
    {
        return (Quaternion.Euler(0, Random.Range(0,360), 0) * Vector3.forward).normalized;
    }

    Vector3 GetRandomPosition(Vector3 center, float minDistanceToCenter, float maxDistanceToCenter)
    {
        return TerrainHelper.AdjustPositionToFloor(center + GetRandomDirection() * 
            GetRandomDistance(minDistanceToCenter, maxDistanceToCenter));
    }


    void Spawn(int numberOfSpawnPoints)
    {
        if (enemyPool.Count == maxAmount)
        {
            return;
        }
        else if (numberOfSpawnPoints + enemyPool.Count > maxAmount)
        {
            numberOfSpawnPoints = enemyPool.Count - maxAmount;
        }

        for (int i = 0; i < numberOfSpawnPoints; ++i)
        {
            SpawnGroup(Random.Range(1, maxAmountInAGroup + 1));
        }
    }

    void SpawnGroup(int amount)
    {
        var mainClone = ObjectPoolManager.GetInstance().GetObjectFromPool(prefab.tag);
        mainClone.GetComponent<EnemyStats>().aggresiveness = Random.Range(1f, 9f);
        mainClone.transform.position = GetRandomPosition(GameObjectRefs.player.position, minDistance, maxDistance);
        enemyPool.Add(mainClone);

        for (int i = 0; i < amount; ++i)
        {
            var subClone = ObjectPoolManager.GetInstance().GetObjectFromPool(prefab.tag);
            subClone.GetComponent<EnemyStats>().aggresiveness = mainClone.GetComponent<EnemyStats>().aggresiveness + Random.Range(-1,1);
            subClone.transform.position = GetRandomPosition(mainClone.transform.position,
                minDistanceInEnemyGroups, maxDistanceInEnemyGroups);
        }
    }


}
