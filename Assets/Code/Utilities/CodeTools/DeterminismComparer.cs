using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class DeterminismComparer : MonoBehaviour
{
    List<Vector3Ser> positionData;
    List<Vector3Ser> prevPositionData;
    List<float[,]> noiseMaps;
    List<float[,]> prevNoiseMaps;

    const string noiseMapFileName = "noiseMaps.dat";
    const string positionDataFileName = "positionData.dat";


    private static DeterminismComparer instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        positionData = new List<Vector3Ser>();
        noiseMaps = new List<float[,]>();

        prevPositionData = Serializer.Deserialize<List<Vector3Ser>>(positionDataFileName);
        prevNoiseMaps = Serializer.Deserialize<List<float[,]>>(noiseMapFileName);
    }

    public static DeterminismComparer GetInstance()
    {
        return instance;
    }

    public void AddNoiseMap(float[,] noiseMap)
    {
        noiseMaps.Add(noiseMap);
    }

    public bool CompareNoiseMaps()
    {
        int failNoiseMaps = 0;

        if(noiseMaps.Count != prevNoiseMaps.Count)
        {
            Debug.Log("Noisemap and prev are not the same length. Prev length: "+prevNoiseMaps.Count+" Noisemap length: "+noiseMaps.Count);
            return false;
        }

        int counter = 0;
        for (int i = 0; i < noiseMaps.Count; i++)
        {
            if (!IsNoiseMapInNoiseMaps(noiseMaps[i], prevNoiseMaps))
            {
                failNoiseMaps = counter;
            }
            counter++;
        }

        if(failNoiseMaps > 0)
        {
            Debug.Log("Noise map failed: " + failNoiseMaps + " out of: " + noiseMaps.Count);
            return false;
        }

        return true;
    }

    bool IsNoiseMapInNoiseMaps(float[,] map, List<float[,]> maps)
    {
        foreach(var item in maps)
        {
            if(CompareFloatArrays(map, item))
            {
                return true;
            }
        }
        return false;
    }

    bool CompareFloatArrays(float[,] a1, float[,] a2)
    {
        if(a1.GetLength(0) != a2.GetLength(0) ||
           a1.GetLength(1) != a2.GetLength(1))
        {
            return false;
        }
        for (int x = 0; x < a1.GetLength(0); x++)
        {
            for (int y = 0; y < a1.GetLength(1); y++)
            {
                if(a1[x,y] != a2[x,y])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void AddFinalData(List<Vector3> newData)
    {
        /*var test = new List<Vector3>();
        test.Add(new Vector3(1, 1, 1));
        test.Add(new Vector3(1, 2, 3));
        test.Add(new Vector3(1.434f, 2.6456f, 3.345f));
        newData = test;
        positionData = new List<Vector3Ser>();*/

        foreach (var pos in newData)
        {
            positionData.Add(new Vector3Ser(pos.x, pos.y, pos.z));
        }
    }

    private void OnApplicationQuit()
    {
        Serializer.DeleteFile(positionDataFileName);
        Serializer.DeleteFile(noiseMapFileName);

        Serializer.Serialize(positionData, positionDataFileName);
        Serializer.Serialize(noiseMaps, noiseMapFileName);
    }

    bool IsPositionInPosList(Vector3Ser position, List<Vector3Ser> data)
    {
        foreach(var pos in data)
        {
            if(Vector3Ser.Compare(position, pos))
            {
                return true;
            }
        }
        return false;
    }

    public bool ComparePositionData()
    {
        if(positionData.Count != positionData.Count)
        {
            return false;
        }

        foreach(var pos in positionData)
        {
            if(!IsPositionInPosList(pos, prevPositionData))
            {
                Debug.Log("Difference: " + pos.ToString());
                return false;
            }
        }

        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DeterminismComparer))]
public class DeterminismComparerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DeterminismComparer comp = (DeterminismComparer)target;

        if(GUILayout.Button("Compare positionData"))
        {
            if(comp.ComparePositionData())
            {
                Debug.Log("Positions are the same");
            }
            else
            {
                Debug.Log("Positions are NOT the same");
            }
        }

        if (GUILayout.Button("Compare NoiseMaps"))
        {
            if (comp.CompareNoiseMaps())
            {
                Debug.Log("NoiseMaps are the same");
            }
            else
            {
                Debug.Log("NoiseMaps are NOT the same");
            }
        }
    }
}
#endif