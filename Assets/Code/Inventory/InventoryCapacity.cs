using UnityEngine;
using System.Collections;
using ItemEnum = GameData.Enums.Items;
using GameData.Converters;
using System.Collections.Generic;

namespace Utils
{
    public static class InventoryCapacity
    {
        public static readonly Dictionary<ItemEnum, int> inventoryCapacity = new Dictionary<ItemEnum, int>
    {
        {ItemEnum.Stick, 6},
        {ItemEnum.Stone, 3},
        {ItemEnum.Rag, 10},
        {ItemEnum.Fish, 3},
        {ItemEnum.CookedFish, 3},
        {ItemEnum.FireTorch, 1},
        {ItemEnum.Flashlight, 1},
        {ItemEnum.Blueberry, 6 },
        {ItemEnum.Rope, 3},
        {ItemEnum.NullItem, -1},
        {ItemEnum.EmptyCan, 1},
        {ItemEnum.FilledCan, 1},
        {ItemEnum.FilledBoiledCan, 1},
        {ItemEnum.Branch, 6}
    };

        public static int GetInventoryCapacity(ItemEnum item)
        {
            if (!inventoryCapacity.ContainsKey(item))
            {
                Debug.LogError("[PlayerStats] Item not set in inventory capcity");
                return 0;
            }
            return inventoryCapacity[item];
        }

        public static bool HasReachedLimit(ItemEnum item)
        {
            if (item.Equals(ItemEnum.NullItem))
            {
                return true;
            }

            if (GetInventoryCapacity(item) > Inventory.GetInstance().GetCount(item))
            {
                return false;
            }

            if(CheckCan(item))
            {
                return false;
            }


            Debug.Log("[InventoryCapacityHelper] inventory full for item: " + item);
            return true;
        }

        public static bool HasReachedItemLimit(string tag)
        {
            if (IsStationaryItem(tag))
            {
                return false;
            }

            var item = TagToItem.Get(tag);
            return HasReachedLimit(TagToItem.Get(tag));
        }

        static bool CheckCan(ItemEnum item)
        {
            if (!Inventory.GetInstance().ContainsFilledCan())
            {
                return true;
            }

            return item.Equals(ItemEnum.FilledCan) || item.Equals(ItemEnum.FilledBoiledCan);
        }


        static bool IsStationaryItem(string tag)
        {
            ItemEnum item = TagToItem.Get(tag);
            if(item.Equals(ItemEnum.NullItem))
            {
                return false;
            }
            if(!inventoryCapacity.ContainsKey(item))
            {
                return true;
            }
            return false;
        }
    }
}
