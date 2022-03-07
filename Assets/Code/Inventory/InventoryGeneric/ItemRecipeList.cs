using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecipeList<T>
{
    private List<Item<T>> items = new List<Item<T>>();

    public struct Item<Type>
    {
        public Type id;
        public ObjectAmountList<Type> recipe;
        public float bluePrintOffset;
        public string bluePrintTag;


        public Item(Type id, ObjectAmountList<Type> recipe, float bluePrintOffset, string bluePrintTag)
        {
            this.id = id;
            this.recipe = recipe;
            this.bluePrintOffset = bluePrintOffset;
            this.bluePrintTag = bluePrintTag;
        }

        public Item(Type id) : this()
        {
            this.id = id;
        }

        public Item(Type id, ObjectAmountList<Type> recipe) : this(id)
        {
            this.recipe = recipe;
        }
    }

    public bool Contains(T id)
    {
        foreach(var it in items)
        {
            if(it.id.Equals(id))
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(T id)
    {
        if(Contains(id))
        {
            Debug.LogWarning("[ItemSheet] The item you are trying to add is already in the sheet, item: "+id.ToString());
            return;
        }

        items.Add(new Item<T>(id));
    }

    public void AddItem(T id, ObjectAmountList<T> recipe)
    {
        if (Contains(id))
        {
            Debug.LogWarning("[ItemSheet] The item you are trying to add is already in the sheet, item: " + id.ToString());
            return;
        }

        items.Add(new Item<T>(id, recipe));
    }

    public void AddItem(T id, ObjectAmountList<T> recipe, float bluePrintOffset, string bluePrintTag)
    {
        if (Contains(id))
        {
            Debug.LogWarning("[ItemSheet] The item you are trying to add is already in the sheet, item: " + id.ToString());
            return;
        }

        items.Add(new Item<T>(id, recipe, bluePrintOffset, bluePrintTag));
    }


    public Item<T> GetCraftableItem(ObjectAmountList<T> craftingList)
    {
        foreach (Item<T> item in items)
        {
            if(item.recipe.GetDictionary().Count == craftingList.GetDictionary().Count && 
               !item.recipe.GetDictionary().Except(craftingList.GetDictionary()).Any())
            {
                return item;
            }
        }
        return new Item<T>();
    }


    public List<Item<T>> GetCraftableItems(ObjectAmountList<T> itemList)
    {
        List<Item<T>> craftableItems = new List<Item<T>>();

        foreach (var item in items)
        {
            if (DoesRecipeListContainsItemList(item.recipe, itemList))
            {
                craftableItems.Add(item);
            }
        }
 
        return craftableItems;
    }

    private bool DoesRecipeListContainsItemList(ObjectAmountList<T> recipe, ObjectAmountList<T> itemList)
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
