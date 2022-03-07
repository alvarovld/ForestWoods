using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemEnum = GameData.Enums.Items;





// DEPRECATED. TODO: REMOVE THIS FILE



/*
namespace Assets.Code.Inventory.DataModel
{
    public class ItemList
    {
        // Item item name and amount
        Dictionary<ItemEnum, int> itemList;

        public ItemList()
        {
            itemList = new Dictionary<ItemEnum, int>();
        }


        public void AddItems(ItemEnum item, int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                AddOneItem(item);
            }
        }

        public bool Contains(ItemEnum item)
        {
            return itemList.ContainsKey(item);
        }


        public bool DeleteFromDictionary(Dictionary<ItemEnum, int> otherList)
        {
            foreach (var key in otherList.Keys)
            {
                if (!itemList.ContainsKey(key))
                {
                    Debug.LogWarning("[Inventory] ItemList does not contain the item you are trying to delete: " + key);
                    return false;
                }
                int value = itemList[key];
                value -= otherList[key];
                if (value < 0)
                {
                    Debug.LogWarning("[Inventory] Trying to delete more items than itemList has: " + key);
                    return false;
                }
                itemList[key] = value;
            }
            return true;
        }


        public Dictionary<ItemEnum, int> GetDictionary()
        {
            return itemList;
        }

        public ItemList(ItemList copy)
        {
            this.itemList = new Dictionary<ItemEnum, int>();
            foreach (var it in copy.GetDictionary())
            {
                this.itemList.Add(it.Key, it.Value);
            }
        }

        override
        public string ToString()
        {
            string listToString = "";
            foreach (var key in itemList.Keys)
            {
                int amount = GetCount(key);
                listToString += key + ": " + amount + " ";
            }
            return listToString;
        }

        public void AddOneItem(ItemEnum item)
        {
            if (itemList.ContainsKey(item))
            {
                int amount = itemList[item];
                itemList[item] = ++amount;
                return;
            }
            itemList.Add(item, 1);
        }


        public bool RemoveOneItem(ItemEnum item)
        {
            if (!itemList.ContainsKey(item) || itemList[item] <= 0)
            {
                Debug.LogWarning("[Inventory] the item you are trying to remove does not exist, item: " + item);
                return false;
            }

            itemList[item] = --itemList[item];

            if (itemList[item] < 1)
            {
                itemList.Remove(item);
            }
            return true;
        }

        public int GetCount(ItemEnum item)
        {
            if (!itemList.ContainsKey(item))
            {
                return 0;
            }

            return itemList[item];
        }
    }
}*/