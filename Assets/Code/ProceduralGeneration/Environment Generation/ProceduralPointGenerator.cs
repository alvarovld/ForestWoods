using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class ProceduralPointGenerator
{
    static bool IsPointOutOfBounds(Vector2 point, float sideSize)
    {
        Func<float, bool> IsCoordOutOfBounds = (float coord) =>
        {
            if (coord <= 0 ||
                coord >= sideSize)
            {
                return true;
            }
            return false;
        };

        if(IsCoordOutOfBounds(point.x) || IsCoordOutOfBounds(point.y))
        {
            return true;
        }
        return false;
    }

    static Vector2 AdjustPointWhenIsBorder(Vector2 point, float sideSize)
    {
        float x = AdjustCoordWhenIsBorder(point.x, sideSize);
        float y = AdjustCoordWhenIsBorder(point.y, sideSize);

        return new Vector2(x, y);
    }

    static float AdjustCoordWhenIsBorder(float coord, float sideSize)
    {
        const float epsilon = 8f;

        if (coord <= 0)
        {
            return epsilon;
        }
        
        if (coord >= sideSize)
        {
            return sideSize - epsilon;
        }

        return coord;
    }

    static Vector2 GridToWorldScale(int x, int y, float cellSize, int cellSideSize)
    {
        float fixedX = AdjustCoordWhenIsBorder(x, cellSideSize);
        float fixedY = AdjustCoordWhenIsBorder(y, cellSideSize);
        return new Vector2(x * cellSize, y * cellSize);
    }


    static float[] GetVariationMapForEveryXAndYCoord(int objectsCount, float objectCellSize, float variationFactor, int seed)
    {
        SystemRandom random = new SystemRandom(seed);
        float[] variationMap = new float[objectsCount * 2];
        for (int i = 0; i < objectsCount * 2; i++)
        {
            float variation = random.Range(-objectCellSize * variationFactor, objectCellSize * variationFactor);
            variationMap[i] = variation;
        }
        return variationMap;
    }


    public static int GetHeightMapSizeBasedOnGrid(float radius, float chunkSideSize)
    {
        float cellSize = radius / Mathf.Sqrt(2);
        int cellSideCount = Mathf.FloorToInt(chunkSideSize / cellSize);
        return cellSideCount;
    }

    public static List<Vector2> GeneratePoints(float radius, int chunkSideSize, float[,] heightMap,
    float cutHeight, float variationFactor, int seed, float heightMargin)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        List<Vector2> candidatePoints = new List<Vector2>();

        int cellSideCount = Mathf.FloorToInt(chunkSideSize / cellSize);


        int index = 0;
        for (int x = 0; x < cellSideCount + 1; ++x)
        {
            for (int y = 0; y < cellSideCount + 1; ++y)
            {
                int fixedX = Mathf.Clamp(x - 1, 0, heightMap.GetLength(0) - 1);
                int fixedY = Mathf.Clamp(y - 1, 0, heightMap.GetLength(0) - 1);

                float height = heightMap[fixedX, fixedY];

                index++;
                if ((height + heightMargin) > cutHeight && (height - heightMargin) < cutHeight)
                {
                    candidatePoints.Add(GridToWorldScale(x, y, cellSize, cellSideCount));
                }
            }
        }

        return GetPointsWithVariation(candidatePoints, cellSize, variationFactor, seed);
    }


    static List<float> Array2DToList(float[,] array)
    {
        List<float> list = new List<float>();
        for(int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                list.Add(array[x, y]);
            }
        }
        return list;
    }


    static List<Vector2> GetPointsWithVariation(List<Vector2> points, float cellSize, float variationFactor, int seed)
    {
        float[] variationMap = GetVariationMapForEveryXAndYCoord(points.Count, cellSize, variationFactor, seed);

        int pointsIndex = 0;
        for (int i = 0; i < variationMap.GetLength(0); i++)
        {
            Vector2 variation = new Vector2(variationMap[i], variationMap[i + 1]);

            var candidate = points[pointsIndex] + variation;

            points[pointsIndex] = candidate;

            pointsIndex++;

            if (pointsIndex >= points.Count)
            {
                break;
            }
        }
        return points;
    }
}
