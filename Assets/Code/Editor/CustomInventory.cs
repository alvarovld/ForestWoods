using GameData.Converters;
using UnityEngine;
using ItemEnum = GameData.Enums.Items;
public class CustomInventory : MonoBehaviour
{
    public int sticks;  
    public int stones;
    public int rags;
    public int fishes;
    public bool flashlight;
    public bool fireTorch;
    public bool fillInventoryOnStart;

    public GameObject lightObj;
    public Transform itemParent;

    [Header("Custom Item")]
    public string itemTag;
    public int itemAmount;

    public void UpdateInventory()
    {
        var inventory = Inventory.GetInstance();
        inventory.AddItems(ItemEnum.Stick, sticks);
        inventory.AddItems(ItemEnum.Stone, stones);
        inventory.AddItems(ItemEnum.Fish, fishes);
        inventory.AddItems(TagToItem.Get(itemTag), itemAmount);

        if (flashlight)
        {
            inventory.AddOneItem(ItemEnum.Flashlight);
        }
        if (fireTorch)
        {
            inventory.AddOneItem(ItemEnum.FireTorch);
        }
        inventory.AddItems(ItemEnum.Rag, rags);

        sticks = stones = rags = 0;
        flashlight = fireTorch = false;
    }

    public void LightOnOFF()
    {
        if(lightObj.gameObject.activeSelf)
        {
            lightObj.SetActive(false);
        }
        else
        {
            lightObj.SetActive(true);

        }
    }

    public void ShowInventory()
    {
        Debug.Log("[CustomInventory] " + Inventory.GetInstance().GetItemList().ToString());
    }

    public void Clear()
    {
        Inventory.GetInstance().Clear();
    }

    public void FillInventory()
    {
        var inventory = Inventory.GetInstance();
        inventory.AddItems(ItemEnum.Stick, 99);
        inventory.AddItems(ItemEnum.Stone, 99);
        inventory.AddItems(ItemEnum.Rag, 99);
        inventory.AddOneItem(ItemEnum.FireTorch);
        inventory.AddOneItem(ItemEnum.Flashlight);
        inventory.AddItems(ItemEnum.Fish, 3);
        inventory.AddItems(ItemEnum.CookedFish, 3);

    }

    public void FillUIInventory()
    {
        EnableChildItemsBasedOnInventory.Execute(itemParent);
    }

    public void ShowCraftingList()
    {
        Debug.Log("[CustomInventory] " + InventoryDraft.GetInstance().GetCraftingList().ToString());
    }

    public void ShowInventoryDraft()
    {
        Debug.Log("[CustomInventory] " + InventoryDraft.GetInstance().GetNewInventory().ToString());
    }
}