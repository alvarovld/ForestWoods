using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemAmountDic = System.Collections.Generic.Dictionary<string, int>;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    private Dictionary<string, ObjectPoolReference> poolsDic = new Dictionary<string, ObjectPoolReference>();
    GameObject editorPool = null;

    public bool setPoolParent;
    public int maxNumberOfChilds;
    int poolCount;

    public int maxNumberOfEnabledObjects;

    //[System.Serializable]
    //public struct PreloadedObject
    //{
    //    public GameObject prefab;
    //    public int amount;
    //}

    // Deprecated
    //[Header("Starting pool")]
    //public List<PreloadedObject> preloadedObjects;


    private void Start()
    {
        maxNumberOfChilds = 0;
        poolCount = 0;
    }

    public void Empty()
    {
        foreach(var key in poolsDic.Keys)
        {
            var pool = ((ObjectPool)poolsDic[key].Get()).objPool;
            for(int i = 0; i < pool.Count; ++i)
            {
                var obj = pool.Dequeue();
                Destroy(obj);
            }
        }
    }


    public ItemAmountDic GetAmount()
    {
        ItemAmountDic data = new ItemAmountDic();
        foreach(var key in poolsDic.Keys)
        {
            ObjectPool pool = (ObjectPool)poolsDic[key].Get();
            data.Add(key, pool.Count());
        }

        return data;
    }

    IEnumerator DisableObjectsWhenReachedLimit()
    {
        yield return new WaitUntil(() => poolCount >= maxNumberOfChilds);
        DisableObjects();
        StartCoroutine(DisableObjectsWhenReachedLimit());
    }


    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
            return;
        }

        poolsDic = new Dictionary<string, ObjectPoolReference>();
        if (!editorPool)
        {
            editorPool = new GameObject("Pool");
        }
    }

    void DisableObjects()
    {
        foreach (var key in poolsDic.Keys)
        {
            foreach(var element in ((ObjectPool)poolsDic[key].Get()).objPool)
            {
                element.SetActive(false);
            }
        }
    }

    public int Count()
    {
        int count = 0;
        foreach (var key in poolsDic.Keys)
        {
            count += ((ObjectPool)poolsDic[key].Get()).objPool.Count;
        }
        return count;
    }


    public void LoadPreloadedObjects(ObjectAmountList<string> objectList)
    {
        List<GameObject> preloaded = new List<GameObject>();

        foreach (var tag in objectList.list.Keys)
        {
            int amount = objectList.GetCount(tag);
            for (int i = 0; i < amount; i++)
            {
                preloaded.Add(GetObjectFromPool(tag));
            }
        }

        Debug.Log("[ObjectPoolManager] Loaded " + preloaded.Count + " objects");
        ReturnListObjectToPoolDisabling(preloaded);
    }


    public static ObjectPoolManager GetInstance()
    {
        return instance;
    }


    public GameObject GetDisabledObjectFromPool(string tag, Vector3 position)
    {
        var newObj = GetDisabledObjectFromPool(tag);

        if (newObj == null)
        {
            return null;
        }

        newObj.transform.position = position;
        poolCount -= 1;
        if(poolCount < 0)
        {
            poolCount = 0;
        }

        return newObj;
    }

    public GameObject GetDisabledObjectFromPool(string tag)
    {
        if (tag == "" || tag == "Untagged")
        {
            Debug.LogError("[ObjectPoolManager] Tag of prefab not set or reference not set, tag: "+tag);
            return null;
        }


        if (!poolsDic.ContainsKey(tag))
        {
            ObjectPool pool = new ObjectPool();
            poolsDic.Add(tag, new ObjectPoolReference(
                () => pool,
                val => { pool = (ObjectPool)val; }));
        }

        GameObject newObj = ((ObjectPool)poolsDic[tag].Get()).GetDisabledObjectFromPool(tag);

        if (!newObj)
        {
            return null;
        }

        poolCount -= 1;
        if (poolCount < 0)
        {
            poolCount = 0;
        }

        return newObj;
    }


    public GameObject GetObjectFromPool(string tag)
    {
        var newObj = GetDisabledObjectFromPool(tag);
        if(newObj == null)
        {
            return null;
        }

        if (!newObj.activeSelf)
        {
            newObj.SetActive(true);
        }

        poolCount -= 1;
        if (poolCount < 0)
        {
            poolCount = 0;
        }

        return newObj;
    }

    public GameObject GetObjectFromPool(string tag, Vector3 position)
    {
        var newObj = GetDisabledObjectFromPool(tag);

        if (newObj == null)
        {
            return null;
        }

        newObj.transform.position = position;
        newObj.SetActive(true);
        poolCount -= 1;
        if (poolCount < 0)
        {
            poolCount = 0;
        }

        return newObj;
    }

    public void ReturnListObjectToPoolDisabling(List<GameObject> objList)
    {
        foreach(var obj in objList)
        {
            ReturnObjectToPoolDisabling(obj);
        }

        poolCount += objList.Count;
    }

    public void ReturnListObjectToPoolNotDisabling(List<GameObject> objList)
    {
        foreach (var obj in objList)
        {
            ReturnObjectToPoolNotDisabling(obj);
        }

        poolCount += objList.Count;
    }


    void InnerReturnToPool(GameObject obj)
    {
#if UNITY_EDITOR
        if (setPoolParent)
        {
            obj.transform.SetParent(editorPool.transform);
        }
#endif
        if (!poolsDic.ContainsKey(obj.tag))
        {
            ObjectPool pool = new ObjectPool();
            poolsDic.Add(obj.tag, new ObjectPoolReference(
                () => pool,
                val => { pool = (ObjectPool)val; }));
        }

        poolCount += 1;
    }    

    public void ReturnObjectToPoolNotDisabling(GameObject obj)
    {
        InnerReturnToPool(obj);
        ((ObjectPool)poolsDic[obj.tag].Get()).ReturnObjectToPoolNotDisabling(obj);
    }


    public void ReturnObjectToPoolDisabling(GameObject obj)
    {
        InnerReturnToPool(obj);
        ((ObjectPool)poolsDic[obj.tag].Get()).ReturnObjectToPoolDisabling(obj);
    }

    sealed class ObjectPoolReference
    {
        public Func<object> Get { get; private set; }
        public Action<object> Set { get; private set; }
        public ObjectPoolReference(Func<object> getter, Action<object> setter)
        {
            Get = getter;
            Set = setter;
        }
    }
}
