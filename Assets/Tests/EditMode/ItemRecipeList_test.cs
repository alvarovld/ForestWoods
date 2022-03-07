using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Item = GameData.Enums.Items;
namespace Tests
{
    public class ItemsSheet_test
    {

        ItemRecipeList<Item> sheet;

        [SetUp]
        public void SetUp()
        {
            sheet = new ItemRecipeList<Item>();
        }




        [UnityTest]
        public IEnumerator AddOneRawItem()
        {
            sheet.AddItem(Item.Stick);
            Assert.IsTrue(sheet.Contains(Item.Stick));

            yield return null;

        }

        [UnityTest]
        public IEnumerator GetCraftingList()
        {
            sheet.AddItem(Item.Bed, GetBed(), GameData.Parameters.bluePrintOffset, GameData.Tags.BedBluePrint);
            ItemRecipeList<Item>.Item<Item> item = sheet.GetCraftableItem(GetBed());
            Assert.AreEqual(item.id, Item.Bed);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetCraftableItemsThatContainSomeItem()
        {
            sheet.AddItem(Item.Bed, GetBed(), GameData.Parameters.bluePrintOffset, GameData.Tags.BedBluePrint);
            sheet.AddItem(Item.Rope, GetRope());

            ObjectAmountList<Item> list = new ObjectAmountList<Item>();
            list.AddOneObject(Item.Rag);

            List<ItemRecipeList<Item>.Item<Item>> craftableItems = sheet.GetCraftableItems(list);
            Assert.IsTrue(craftableItems.Count == 2); 
            Assert.IsTrue(craftableItems[0].id == Item.Rope || craftableItems[0].id == Item.Bed);
            Assert.IsTrue(craftableItems[1].id == Item.Rope || craftableItems[1].id == Item.Bed);


            yield return null;
        }



        public void InitializeData()
        {
            sheet.AddItem(Item.Stick);
            sheet.AddItem(Item.Stone);
            sheet.AddItem(Item.Rag);
            sheet.AddItem(Item.Branch);
            sheet.AddItem(Item.Blueberry);
            sheet.AddItem(Item.Flashlight);

            sheet.AddItem(Item.FireTorch, GetFireTorch());
            sheet.AddItem(Item.Rope, GetRope());

            sheet.AddItem(Item.Campfire, GetCampFire(), GameData.Parameters.bluePrintOffset, GameData.Tags.CampFireBluePrint);
            sheet.AddItem(Item.Bed, GetBed(), GameData.Parameters.bluePrintOffset, GameData.Tags.BedBluePrint);
        }
        private static ObjectAmountList<Item> GetFireTorch()
        {
            ObjectAmountList<Item> itemList = new ObjectAmountList<Item>();
            itemList.AddObjects(Item.Stick, 1);
            itemList.AddObjects(Item.Rag, 3);
            return itemList;
        }

        private static ObjectAmountList<Item> GetBed()
        {
            ObjectAmountList<Item> itemList = new ObjectAmountList<Item>();
            itemList.AddObjects(Item.Rag, 3);
            itemList.AddObjects(Item.Branch, 2);
            return itemList;
        }

        private static ObjectAmountList<Item> GetRope()
        {
            ObjectAmountList<Item> itemList = new ObjectAmountList<Item>();
            itemList.AddObjects(Item.Rag, 3);
            return itemList;
        }

        private static ObjectAmountList<Item> GetCampFire()
        {
            ObjectAmountList<Item> itemList = new ObjectAmountList<Item>();
            itemList.AddObjects(Item.Stick, 3);
            itemList.AddOneObject(Item.Stone);
            return itemList;
        }

    }
}
