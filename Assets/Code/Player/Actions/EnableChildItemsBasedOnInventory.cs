using System;
using System.Collections.Generic;
using UnityEngine;
public static class EnableChildItemsBasedOnInventory
{
    public static void Execute(Transform itemParent)
    {
        for(int i = 0; i < itemParent.childCount; ++i)
        {
            var itemGroup = itemParent.GetChild(i);
            var referencedItemObjectList = GetChildObjectsAsList(itemGroup);
            EnableObjectsInReferencedListBasedOnInventory(referencedItemObjectList, itemGroup.tag);
        }
    }


    public static void Reset(Transform itemParent)
    {
        for (int i = 0; i < itemParent.childCount; ++i)
        {
            var itemGroup = itemParent.GetChild(i);
            var referencedItemObjectList = GetChildObjectsAsList(itemGroup);
            EnableObjectsInReferencedListBasedOnInventory(referencedItemObjectList, itemGroup.tag);
            ResetPositionOfObjects(referencedItemObjectList);
        }
    }

    static void ResetPositionOfObjects(List<Transform> objList)
    {
        foreach(var obj in objList)
        {
            obj.transform.position = obj.GetComponent<InventoryItemMouseAction>().initialPosition;
        }
    }

    static void EnableObjectsInReferencedListBasedOnInventory(List<Transform> referencedItemObjectList, string typeTag)
    {
        var amount = Inventory.GetInstance().GetCount(typeTag);
        InnerEnableObjectsInReferencedListBasedOnInventory(referencedItemObjectList, amount);
    }

    static void InnerEnableObjectsInReferencedListBasedOnInventory(List<Transform> referencedItemObjectList, int amount)
    {
        var maxAmount = amount < referencedItemObjectList.Count ? amount : referencedItemObjectList.Count;
        for (int i = 0; i < referencedItemObjectList.Count; ++i)
        {
            var obj = referencedItemObjectList[i].gameObject;
            obj.SetActive(false);

            if (i < maxAmount)
            {
                obj.SetActive(true);
            }
        }
    }

    static List<Transform> GetChildObjectsAsList(Transform itemGroup)
    {
        List<Transform> referenceList = new List<Transform>();
        for (int i = 0; i < itemGroup.childCount; ++i)
        {
            referenceList.Add(itemGroup.transform.GetChild(i));
        }
        return referenceList;
    }
}
