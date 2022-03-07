using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;
using System.Linq;
using System;

public class ProceduralObjectGenerator 
{
    // Placement data
    int chunkSideSize;

    private ProceduralObjectGenerator() { }

    public ProceduralObjectGenerator(int chunkSideSize)
    {
        this.chunkSideSize = chunkSideSize;
    }

    public List<GameObject> GenerateWithoutPool(Vector2 chunkPosition, Transform parent, ProceduralObjectData data, 
        float[,] heightMap, int seed, float heightMargin)
    {
        data.noiseData.offset = chunkPosition;

        List<Vector2> points = ProceduralPointGenerator.GeneratePoints(data.radius, chunkSideSize, heightMap,
                               data.cutHeight, data.gridVariability, seed, heightMargin);

        List<GameObject> gameObjects = new List<GameObject>();

        SystemRandom random = new SystemRandom(seed);
        for (int i = 0; i < Mathf.Clamp(data.maxAmount, 0, points.Count); ++i)
        {
            var point = points[i];
            Vector3 finalPosition = GetPositionInWorldView(new Vector3(point.x, 0, point.y), chunkPosition, data.heightFix);
            gameObjects.Add(InstantiatePrefabWithoutPool(finalPosition, parent, data, random));
        }
        return gameObjects;
    }

    public List<GameObject> Generate(Transform parent, List<GameObjectDataHolder> dataHolder)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (var data in dataHolder)
        {
            var obj = ObjectPoolManager.GetInstance().GetObjectFromPool(data.tag);
            obj.transform.rotation = data.rotation;

            obj.transform.localScale = data.scale;
            obj.transform.position = data.position;
            if (parent != null)
            {
                obj.transform.SetParent(parent);
            }
            gameObjects.Add(obj);
        }

        return gameObjects;
    }

    public List<GameObject> Generate(Vector2 chunkPosition, Transform parent, ProceduralObjectData data, float[,] heightMap, int seed, float heightMargin)
    {
        data.noiseData.offset = chunkPosition;
        List<Vector2> points = ProceduralPointGenerator.GeneratePoints(data.radius, chunkSideSize, heightMap,
                               data.cutHeight, data.gridVariability, seed, heightMargin);

        List<GameObject> gameObjects = new List<GameObject>();

        SystemRandom random = new SystemRandom(seed); 
        for (int i = 0; i < Mathf.Clamp(data.maxAmount, 0, points.Count); ++i)
        {
            var point = points[i];
            Vector3 finalPosition = GetPositionInWorldView(new Vector3(point.x, 0, point.y), chunkPosition, data.heightFix);
            gameObjects.Add(InstantiatePrefabWithPool(finalPosition, parent, data, random));
        }
        return gameObjects;
    }


    GameObject InstantiatePrefabWithoutPool(Vector3 position, Transform parent, ProceduralObjectData data, SystemRandom random)
    {
        GameObject obj = UnityEngine.Object.Instantiate(data.prefab, position, GetRandomRotation(random));
        obj.transform.position = position + Vector3.up * data.heightFix;
        float scale = random.Range(data.minScale, data.maxScale);
        obj.transform.position += Vector3.up * data.heightFix;
        obj.transform.SetParent(parent);
        return obj;
    }

    GameObject InstantiatePrefabWithPool(Vector3 position, Transform parent, ProceduralObjectData data, SystemRandom random)
    {
        var obj = ObjectPoolManager.GetInstance().GetObjectFromPool(data.prefab.tag, position);
        obj.transform.rotation = GetRandomRotation(random);
        obj.transform.position = TerrainHelper.AdjustPositionToFloor(obj) + Vector3.up * data.heightFix;
        if (data.minScale != 1 && data.maxScale != 1)
        {
            float scale = random.Range(data.minScale, data.maxScale);
            obj.transform.localScale = Vector3.one * scale;
        }
        obj.transform.SetParent(parent);
        return obj;
    }

    Quaternion GetRandomRotation(SystemRandom rnd)
    {
        return Quaternion.Euler(0, rnd.Range(0, 360), 0);
    }

    Vector3 GetPositionInWorldView(Vector3 point, Vector2 chunkPosition, float heightFix)
    {
        float offsetX = chunkPosition.x - chunkSideSize / 2;
        float offsetY = chunkPosition.y - chunkSideSize / 2;
        return new Vector3(point.x + offsetX, point.y + heightFix, point.z + offsetY);
    }
}
