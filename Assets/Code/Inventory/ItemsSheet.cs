using ItemEnum = GameData.Enums.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using ItemList = ObjectAmountList<GameData.Enums.Items>;
public static class ItemsSheet
{
    //private static List<StorageItemGeneric> items = new List<StorageItemGeneric>();
    private static List<ConstructedItem> constructedItems = new List<ConstructedItem>();
    private static List<ConstructedStationaryItem> constructedStationaryItems = new List<ConstructedStationaryItem>();

    public static ConstructedItem GetSheetFor(ItemEnum item)
    {
        foreach(var constructed in constructedItems)
        {
            if(constructed.itemEnum.Equals(item))
            {
                return constructed;
            }
        }
        foreach(var stationry in constructedStationaryItems)
        {
            if (stationry.itemEnum.Equals(item))
            {
                return stationry;
            }
        }

        UnityEngine.Debug.LogWarning("Item not found in constructed or stationary sheets: "+item);
        return new ConstructedItem();
    }

    public static void AddItem(ItemEnum itemEnum)
    {
        //items.Add(new Item(
        //    itemEnum.ToString(), 
        //   itemEnum));
    }

    public static void AddConstructedItem(ItemEnum itemEnum, ItemList recipe)
    {
        constructedItems.Add(new ConstructedItem(
            itemEnum.ToString(),
            recipe, 
            itemEnum));
    }

    public static void AddConstructedStationaryItem(ItemEnum itemEnum, ItemList recipe, float bluePrintOffset, string bluePrintTag)
    {
        constructedStationaryItems.Add(new ConstructedStationaryItem(
            bluePrintOffset, 
            itemEnum.ToString(),
            bluePrintTag,  
            itemEnum, 
            recipe));
    }


    public static ConstructedItem GetCraftableItem(ItemList craftingList)
    {
        Func<ConstructedItem, bool> equivalent = (ConstructedItem item) =>
            {
                return item.recipe.GetDictionary().Count == craftingList.GetDictionary().Count &&
                 !item.recipe.GetDictionary().Except(craftingList.GetDictionary()).Any();
            };

        foreach(var item in constructedItems)
        {
            if(equivalent(item))
            {
                return item;
            }
        }
        foreach(var item in constructedStationaryItems)
        {
            if (equivalent(item))
            {
                return item;
            }
        }
        return new ConstructedItem();
    }

    public static List<ConstructedItem> GetCraftableItemsFromCraftingList()
    {
        return GetCraftableItems(InventoryDraft.GetInstance().GetCraftingList());
    }

    private static List<ConstructedItem> GetCraftableItems(ItemList itemList)
    {
        List<ConstructedItem> craftableItems = new List<ConstructedItem>();

        foreach (var constructed in constructedItems)
        {
            if (DoesRecipeListContainsItemList(constructed.recipe, itemList))
            {
                craftableItems.Add(constructed);
            }
        }
        foreach (var stationary in constructedStationaryItems)
        {
            if (DoesRecipeListContainsItemList(stationary.recipe, itemList))
            {
                craftableItems.Add(stationary);
            }
        }

        return craftableItems;
    }

    static bool DoesRecipeListContainsItemList(ItemList recipe, ItemList itemList)
    {
        foreach (var item in itemList.GetDictionary())
        {
            if (!recipe.Contains(item.Key))
            {
                return false;
            }
            else if (recipe.GetCount(item.Key) < item.Value)
            {
                return false;
            }
        }
        return true;
    }
}
