using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Serializer
{
    public static void Serialize<T>(T obj, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                var bformatter = new BinaryFormatter();
                BinaryFormatter bf1 = new BinaryFormatter();
                bf1.Serialize(stream, obj);
                stream.Close();
            }
        }
        else
        {
            FileStream fs = File.Create(path);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }

    public static T Deserialize<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;

        Debug.Log(path);

        if (!File.Exists(path))
        {
            return default;
        }

        T obj;
        using (Stream stream = File.Open(path, FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            obj = (T)bformatter.Deserialize(stream);
        }
        return obj;
    }

    public static void DeleteFile(string fileName)
    {
        File.Delete(Application.persistentDataPath + "/" + fileName);
    }
}


[System.Serializable]
public class Vector3Ser
{
    public float x, y, z;

    public Vector3Ser(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    override
    public string ToString()
    {
        return "[" + x + ", " + y + ", " + z + "] ";
    }

    public static bool Compare(Vector3Ser v1, Vector3Ser v2)
    {
        if (v1.x == v2.x &&
           v1.z == v2.z)
        {
            return true;
        }
        return false;
    }
}
