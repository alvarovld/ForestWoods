using UnityEngine;
using Utils;
using ItemList = ObjectAmountList<GameData.Enums.Items>;

public class ConstructedStationaryItem : ConstructedItem
{
    public float bluePrintOffset;
    public string bluePrintTag;
    public GameObject bluePrintInstance = null;

    public ConstructedStationaryItem(float bluePrintOffset, string tag, string bluePrintTag,
        GameData.Enums.Items itemEnum, ItemList recipe) : base(tag, recipe, itemEnum)
    {
        this.bluePrintOffset = bluePrintOffset;
        this.bluePrintTag = bluePrintTag;

    }
    GameObject GetBluePrintObj()
    {
        GameObject bluePrint = ResourcesLoader.Load<GameObject>(bluePrintTag);
        if(!bluePrint)
        {
            Debug.LogError("BluePrint not found, tag: " + bluePrintTag);
            return null;
        }
        bluePrint.AddComponent<BluePrint>().SetOffset(bluePrintOffset);
        bluePrint.SetActive(false);
        return bluePrint;
    }


    public void HideBluePrint()
    {
        Object.Destroy(bluePrintInstance);
    }

    public void ShowBluePrint()
    {
        if(bluePrintInstance)
        {
            bluePrintInstance.SetActive(true);
            return;
        }

        var sheet = GetBluePrintObj();
        if (!sheet)
        {
            return;
        }

        bluePrintInstance = Object.Instantiate(sheet);
    }

}
