using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectAmountList<Object>
{
    // Item item name and amount
    public Dictionary<Object, int> list = new Dictionary<Object, int>();

    public ObjectAmountList()
    {
    }

    public int Count()
    {
        return list.Count;
    }

    public void AddObjects(Object item, int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            AddOneObject(item);
        }
    }

    public bool Contains(Object item)
    {
        return list.ContainsKey(item);
    }


    public bool DeleteFromDictionary(Dictionary<Object, int> otherList)
    {
        foreach (var key in otherList.Keys)
        {
            if (!list.ContainsKey(key))
            {
                Debug.LogWarning("[ObjectList] ObjectList does not contain the item you are trying to delete: " + key);
                return false;
            }
            int value = list[key];
            value -= otherList[key];
            if (value < 0)
            {
                Debug.LogWarning("[ObjectList] Trying to delete more items than itemList has: " + key);
                return false;
            }
            list[key] = value;
        }
        return true;
    }


    public Dictionary<Object, int> GetDictionary()
    {
        return list;
    }

    public ObjectAmountList(ObjectAmountList<Object> copy)
    {
        list = new Dictionary<Object, int>();
        foreach (var it in copy.GetDictionary())
        {
            this.list.Add(it.Key, it.Value);
        }
    }

    override
    public string ToString()
    {
        string listToString = "";
        foreach (var key in list.Keys)
        {
            int amount = GetCount(key);
            listToString += key + ": " + amount + " ";
        }
        return listToString;
    }

    public void AddOneObject(Object item)
    {
        if (list.ContainsKey(item))
        {
            int amount = list[item];
            list[item] = ++amount;
            return;
        }
        list.Add(item, 1);
    }


    public bool RemoveOneObject(Object item)
    {
        if (!list.ContainsKey(item) || list[item] <= 0)
        {
            Debug.LogWarning("[ObjwctList] the item you are trying to remove does not exist, object: " + item);
            return false;
        }

        list[item] = --list[item];

        if (list[item] < 1)
        {
            list.Remove(item);
        }
        return true;
    }

    public int GetCount(Object item)
    {
        if (!list.ContainsKey(item))
        {
            return 0;
        }

        return list[item];
    }
}
