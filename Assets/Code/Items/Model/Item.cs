using UnityEngine;
using System.Collections;

public class Item 
{ 
    public string tag;
    public GameData.Enums.Items itemEnum;

    public Item(string tag, GameData.Enums.Items itemEnum)
    {
        this.tag = tag;
        this.itemEnum = itemEnum;
    }
    public Item()
    {
        this.itemEnum = GameData.Enums.Items.NullItem;
    }
}
