using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public Queue<GameObject> objPool = new Queue<GameObject>();
    public void FillPool(GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject newObj = InstantiateWithTracking(prefab);
            objPool.Enqueue(newObj);
            newObj.SetActive(false);
        }
    }

    public int Count()
    {
        return objPool.Count;
    }

    public GameObject GetDisabledObjectFromPool(string tag)
    {
        if (objPool.Count == 0)
        {
            GameObject prefab = ResourcesLoader.Load<GameObject>(tag);
            if(prefab == null)
            {
                Debug.LogError("[ObjectPool] object not found in Resources directory, tag: " + tag);
                return null;
            }
            objPool.Enqueue(InstantiateWithTracking(prefab));
        }
        GameObject newObj = objPool.Dequeue();
        /*if (newObj.transform.childCount == 1)
        {
            newObj.transform.GetChild(0).transform.position = prefab.transform.GetChild(0).transform.position;
        }

        newObj.transform.SetPositionAndRotation(prefab.transform.position, prefab.transform.rotation);*/
        return newObj;
    }


    GameObject InstantiateWithTracking(GameObject prefab)
    {
        var clone = Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        ObjectAmountTracker.GetInstance().AddOneObject(prefab.tag);
        return clone;
    }

    public GameObject GetObjectFromPool(string tag)
    {
        var newObj = GetDisabledObjectFromPool(tag);
        newObj.SetActive(true);
        return newObj;
    }


    public void ReturnObjectToPoolNotDisabling(GameObject obj)
    {
        objPool.Enqueue(obj);
    }

    public void ReturnObjectToPoolDisabling(GameObject obj)
    {
        obj.SetActive(false);   
        if(obj.transform.childCount == 1)
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
        }
        objPool.Enqueue(obj);
    }
}
