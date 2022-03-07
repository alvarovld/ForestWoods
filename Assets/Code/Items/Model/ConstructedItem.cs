using UnityEngine;
using System.Collections;
using ItemList =  ObjectAmountList<GameData.Enums.Items>;

public class ConstructedItem : Item
{
    public ItemList recipe;

    public ConstructedItem(string tag, ItemList recipe, 
        GameData.Enums.Items itemEnum) : base(tag, itemEnum)
    {
        this.recipe = recipe;
    }

    public ConstructedItem()
    {
        itemEnum = GameData.Enums.Items.NullItem;
    }

    public GameObject GetPrefab()
    {
        return ResourcesLoader.Load<GameObject>(tag);
    }

}
