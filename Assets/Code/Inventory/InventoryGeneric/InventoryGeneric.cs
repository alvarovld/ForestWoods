using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryGeneric : MonoBehaviour
{
    public List<Recipe> capacity;
    public Storage<string> inventory;

    [System.Serializable]
    public struct ItemCapacity
    {
        public string item;
        public int capacity;
    }
    [System.Serializable]
    public struct Recipe
    {
        public string item;
        public List<string> recipe;
        public int capacity;
    }


    private void Start()
    {
        var dic = GetItemCapacityDictionary();
        if(dic == null)
        {
            return;
        }

        //inventory = new Storage<string>(GetItemCapacityDictionary());
    }



    public Dictionary<string, int> GetItemCapacityDictionary()
    {
        Dictionary<string, int> cap = new Dictionary<string, int>();
        foreach (var it in capacity)
        {
            if (cap.ContainsKey(it.item))
            {
                Debug.LogWarning("Item is repeated: " + it.item);
                return null;
            }
            cap.Add(it.item, it.capacity);
        }

        return cap;
    }
}

[CustomEditor(typeof(InventoryGeneric))]

public class InventoryGenericEditor : Editor
{

}
