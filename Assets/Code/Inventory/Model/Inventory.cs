
using UnityEngine;

using ItemEnum = GameData.Enums.Items;
using ItemList = ObjectAmountList<GameData.Enums.Items>;
public class Inventory
{
    private static Inventory instance = null;
    private static ItemList inventory;
    private Inventory()
    {
    }
    
    public void Clear()
    {
        inventory = new ItemList();
    }

    public ItemList GetItemList()
    {
        return inventory;
    }

    public ItemList GetInventoryCopy()
    {
        return new ItemList(inventory);
    }

    public bool AddItems(ItemEnum item, int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            if(!AddOneItem(item))
            {
                return false;
            }
        }
        return true;
    }

    public void SetNewInventory(ItemList newList)
    {
        inventory = newList;
    }

    public bool Contains(ItemEnum item)
    {
        return inventory.Contains(item);
    }

    public int GetItemAmount(ItemEnum item)
    {
        return inventory.GetCount(item);
    }

    public bool EmptyCan() 
    {
        if (!InnerEmptyCan(ItemEnum.FilledBoiledCan) && !InnerEmptyCan(ItemEnum.FilledCan))
        {
            Debug.LogWarning("[Inventory] Inventory does not contain a filled can");
            return false;
        }
        return true;
    }

    private static bool InnerEmptyCan(ItemEnum type)
    {
        if (inventory.Contains(type))
        {
            inventory.RemoveOneObject(type);
            inventory.AddOneObject(ItemEnum.EmptyCan);
            return true;
        }
        return false;
    }


    public static Inventory GetInstance()
    {
        if (instance == null)
        {
            instance = new Inventory();
            inventory = new ItemList();
        }
        return instance;
    }

    public bool AddOneItem(ItemEnum item)
    {
        if(Utils.InventoryCapacity.HasReachedLimit(item))
        {
            Debug.LogWarning("[Inventory] inventory has reached limit for " + item);
            return false;
        }
        inventory.AddOneObject(item);
        return true;
    }

    public bool ContainsFilledCan()
    {
        return inventory.Contains(ItemEnum.FilledBoiledCan) || inventory.Contains(ItemEnum.FilledCan);
    }

    public bool RemoveOneItem(ItemEnum item)
    {
        return inventory.RemoveOneObject(item);
    }

    public int GetCount(ItemEnum item)
    {
        return inventory.GetCount(item);
    }

    public int GetCount(string tag)
    {
        var item = GameData.Converters.TagToItem.Get(tag);
        return GetCount(item);
    }

}