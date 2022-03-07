using UnityEngine;
using Utils;
using ItemEnum = GameData.Enums.Items;
using ItemList = ObjectAmountList<GameData.Enums.Items>;

public class InventoryDraft 
{
    private static ItemList newInventory;
    private static ItemList craftingList;
    private static InventoryDraft instance = null;
    private InventoryDraft()
    {
        craftingList = new ItemList();
        newInventory = Inventory.GetInstance().GetInventoryCopy();
    }

    public bool isCraftingListEmpty()
    {
        return GetCraftingList().GetDictionary().Count.Equals(0);
    }

    public ItemList GetNewInventory()
    {
        return newInventory;
    }
    public static InventoryDraft GetInstance()
    {
        if (instance == null)
        {
            instance = new InventoryDraft();
        }
        return instance;
    }

    public ItemList GetCraftingList()
    {
        return craftingList;
    }

    public int GetItemCount(ItemEnum item)
    {
        return newInventory.GetCount(item);
    }

    public bool AddItemToCraftingList(ItemEnum item)
    {
        if (!newInventory.RemoveOneObject(item))
        {
            Debug.LogWarning("[Inventory] Item you are trying to remove does not exist in inventory: " + item);
            return false;
        }
        else if (item.Equals(ItemEnum.NullItem))
        {
            Debug.LogError("[Inventory] Unknown item");
            return false;
        }

        craftingList.AddOneObject(item);
        return true;
    }

    public void RemoveItemFromCraftingList(ItemEnum item)
    {
        if (craftingList.RemoveOneObject(item))
        {
            newInventory.AddOneObject(item);
        }
        else
        {
            Debug.LogWarning("[InventoryDraft] Item you are trying to remove from crafting list does not exist: " + item);
        }
    }

    public void RemoveItemFromCraftingList(string tag)
    {
        RemoveItemFromCraftingList(GameData.Converters.TagToItem.Get(tag));
    }
    public void AddItemToCraftingList(string tag)
    {
        AddItemToCraftingList(GameData.Converters.TagToItem.Get(tag));
    }

    public void ConsolidateCraft()
    {
        ConstructedItem item = GetCraftableItemFromCraftingList();

        if(item.itemEnum.Equals(ItemEnum.NullItem))
        {
            return;
        }
        if(!(item is ConstructedStationaryItem))
        {
            newInventory.AddOneObject(item.itemEnum);
        }

        Inventory.GetInstance().SetNewInventory(newInventory);
        Reset();
    }

    public void Reset()
    {
        craftingList = new ItemList();
        newInventory = Inventory.GetInstance().GetInventoryCopy();
    }

    public string GetCraftingListString()
    {
        return craftingList.ToString();
    }

    public ConstructedItem GetCraftableItemFromCraftingList()
    {
        ConstructedItem item = ItemsSheet.GetCraftableItem(craftingList);
        if(item is ConstructedStationaryItem)
        {
            return item;
        }    

        if(InventoryCapacity.HasReachedLimit(item.itemEnum))
        {
            return new ConstructedItem();
        }
        return item;     
    }



}