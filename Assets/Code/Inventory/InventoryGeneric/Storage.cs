using System.Collections.Generic;
using UnityEngine;

public class Storage<ItemType>
{
    private ObjectAmountList<ItemType> itemList = new ObjectAmountList<ItemType>();

    private Dictionary<ItemType, StorageItemGeneric<ItemType>> itemData = new Dictionary<ItemType, StorageItemGeneric<ItemType>>();

    public Storage()
    {
    }

    public Storage(Dictionary<ItemType, StorageItemGeneric<ItemType>> data)
    {
        itemData = data;
    }

    public void Clear()
    {
        itemList = new ObjectAmountList<ItemType>();
    }

    public ObjectAmountList<ItemType> GetItemList()
    {
        return itemList;
    }

    public int GetStorageCapacity(ItemType item)
    {
        if (!itemData.ContainsKey(item))
        {
            Debug.LogError("[PlayerStats] Item not set in inventory capcity");
            return 0;
        }
        return itemData[item].capacity;
    }

    public bool HasReachedLimit(ItemType item)
    {
        if (GetStorageCapacity(item) > itemList.GetCount(item))
        {
            return false;
        }

        Debug.Log("[InventoryCapacityHelper] inventory full for item: " + item);
        return true;
    }
    public ObjectAmountList<ItemType> GetStorageCopy()
    {
        return new ObjectAmountList<ItemType>(itemList);
    }

    public bool AddElement(ItemType item, int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            if (!AddOneItem(item))
            {
                return false;
            }
        }
        return true;
    }

    public void SetNewStorage(ObjectAmountList<ItemType> newList)
    {
        itemList = newList;
    }

    public bool Contains(ItemType item)
    {
        return itemList.Contains(item);
    }

    public int GetItemAmount(ItemType item)
    {
        return itemList.GetCount(item);
    }


    public bool AddOneItem(ItemType item)
    {
        if (HasReachedLimit(item))
        {
            Debug.LogWarning("[Inventory] inventory has reached limit for " + item);
            return false;
        }
        itemList.AddOneObject(item);
        return true;
    }


    public bool RemoveOneItem(ItemType item)
    {
        return itemList.RemoveOneObject(item);
    }

    public int GetItemCount(ItemType item)
    {
        return itemList.GetCount(item);
    }

}
