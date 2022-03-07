using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ItemEnum = GameData.Enums.Items;
namespace Tests
{
    public class Storage_test
    {
        Storage<GameData.Enums.Items> inventory;

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


        [SetUp]
        public void SetUp()
        {
            //inventory = new Storage<GameData.Enums.Items>(inventoryCapacity);
            //inventory = Inventory.GetInstance();
        }

        [TearDown]
        public void TeardDown()
        {
            inventory.Clear();
        }




        [UnityTest]
        public IEnumerator Clear()
        {
            inventory.AddOneItem(GameData.Enums.Items.Stick);
            inventory.Clear();

            Assert.IsTrue(inventory.GetItemList().GetDictionary().Count == 0);
            yield return null;
        }


        [UnityTest]
        public IEnumerator GetItemAmount()
        {
            inventory.AddOneItem(GameData.Enums.Items.Stick);
            inventory.AddOneItem(GameData.Enums.Items.Stick);
            inventory.AddOneItem(GameData.Enums.Items.Stick);
            inventory.AddOneItem(GameData.Enums.Items.Stone);

            ObjectAmountList<GameData.Enums.Items> itemList = inventory.GetItemList();

            Assert.AreEqual(3, inventory.GetItemAmount(GameData.Enums.Items.Stick));

            yield return null;
        }

        [UnityTest]
        public IEnumerator AddMoreItemsThanAllowed()
        {
            int amount = inventoryCapacity[ItemEnum.Stick];
            inventory.Clear();

            inventory.AddElement(ItemEnum.Stick, amount);

            Assert.IsFalse(inventory.AddOneItem(ItemEnum.Stick));

            yield return null;
        }


        /*[UnityTest]
        public IEnumerator CheckOnlyOneCanAvailable()
        {
            inventory.AddOneItem(GameData.Enums.Items.FilledCan);
            Assert.IsTrue(!inventory.AddOneItem(GameData.Enums.Items.FilledBoiledCan));
            Assert.IsTrue(!inventory.AddOneItem(GameData.Enums.Items.FilledCan));

            Assert.AreEqual(0, inventory.GetItemAmount(GameData.Enums.Items.FilledBoiledCan));
            Assert.AreEqual(0, inventory.GetItemAmount(GameData.Enums.Items.FilledCan));

            yield return null;
        }*/



        /*[UnityTest]
        public IEnumerator EmptyCan()
        {
            inventory.AddOneItem(GameData.Enums.Items.FilledCan);
            Assert.IsTrue(inventory.EmptyCan());
            Assert.IsTrue(
                !inventory.Contains(GameData.Enums.Items.FilledCan) &&
                !inventory.Contains(GameData.Enums.Items.FilledBoiledCan)
                );

            yield return null;
        }*/

        [UnityTest]
        public IEnumerator AddOneItem()
        {
            inventory.AddOneItem(GameData.Enums.Items.Stick);
            ObjectAmountList<GameData.Enums.Items> itemList = inventory.GetItemList();

            Assert.AreEqual(1, itemList.Count());
            Assert.IsTrue(itemList.GetDictionary().ContainsKey(GameData.Enums.Items.Stick));

            yield return null;
        }

        [UnityTest]
        public IEnumerator DeleteOneItem()
        {
            inventory.Clear();
            inventory.AddOneItem(GameData.Enums.Items.Stick);
            inventory.RemoveOneItem(GameData.Enums.Items.Stick);
            Assert.AreEqual(0, inventory.GetItemList().GetDictionary().Count);
            yield return null;
        }
    }
}
