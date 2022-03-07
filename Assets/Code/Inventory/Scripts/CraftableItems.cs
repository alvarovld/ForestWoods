using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemList = ObjectAmountList<GameData.Enums.Items>;
using ItemEnum = GameData.Enums.Items;

public static class CraftableItems
{
    private static ItemRecipeList<ItemEnum> sheet = new ItemRecipeList<ItemEnum>();

    public static void InitializeData()
    {
        sheet.AddItem(ItemEnum.Stick);
        sheet.AddItem(ItemEnum.Stone);
        sheet.AddItem(ItemEnum.Rag);  
        sheet.AddItem(ItemEnum.Branch);
        sheet.AddItem(ItemEnum.Blueberry);
        sheet.AddItem(ItemEnum.Flashlight);

        sheet.AddItem(ItemEnum.FireTorch, GetFireTorch());
        sheet.AddItem(ItemEnum.Rope, GetRope());

        sheet.AddItem(ItemEnum.Campfire, GetCampFire(), GameData.Parameters.bluePrintOffset, GameData.Tags.CampFireBluePrint);
        sheet.AddItem(ItemEnum.Bed, GetBed(), GameData.Parameters.bluePrintOffset, GameData.Tags.BedBluePrint);
    }

    public static ItemRecipeList<ItemEnum>  GetSheet()
    {
        return sheet;
    }

    private static ItemList GetFireTorch()
    {
        ItemList itemList = new ItemList();
        itemList.AddObjects(ItemEnum.Stick, 1);
        itemList.AddObjects(ItemEnum.Rag, 3);
        return itemList;
    }

    private static ItemList GetBed()
    {
        ItemList itemList = new ItemList();
        itemList.AddObjects(ItemEnum.Rag, 3);
        itemList.AddObjects(ItemEnum.Branch, 2);
        return itemList;
    }

    private static ItemList GetRope()
    {
        ItemList itemList = new ItemList();
        itemList.AddObjects(ItemEnum.Rag, 3);
        return itemList;
    }

    private static ItemList GetCampFire()
    {
        ItemList itemList = new ItemList();
        itemList.AddObjects(ItemEnum.Stick, 3);
        itemList.AddOneObject(ItemEnum.Stone);
        return itemList;
    }
}
