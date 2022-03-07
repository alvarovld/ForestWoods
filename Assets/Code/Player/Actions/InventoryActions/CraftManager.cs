using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class CraftManager : MonoBehaviour
{
    static CraftManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static CraftManager GetInstance()
    {
        return instance;
    }

    void SetUpCraftText(GameObject canvas)
    {
        canvas.transform.GetChild(0).GetComponent<Text>().text = "Craft  (" + GameData.Keys.CRAFT + ")\n" +
                                                                 "Cancel (" + GameData.Keys.CANCEL_CRAFT+")";
    }

    bool HandleItemCraft(ConstructedStationaryItem item)
    {
        // Craft
        if (Input.GetKeyDown(GameData.Keys.CRAFT))
        {
            Instantiate(item.GetPrefab(), item.bluePrintInstance.transform.position, 
                item.bluePrintInstance.transform.rotation);
            InventoryDraft.GetInstance().ConsolidateCraft();
            item.HideBluePrint();
            return true;
        }
        // Return
        if (Input.GetKeyDown(GameData.Keys.CANCEL_CRAFT))
        {
            item.HideBluePrint();
            InventoryDraft.GetInstance().Reset();
            return true;
        }
        return false;
    }

    IEnumerator ShowBluePrint(ConstructedStationaryItem item)
    {
        InputHandler.GetInstance().CloseInventory();
        item.ShowBluePrint();

        Func<bool> craftOrReturn = () =>
        {
            return HandleItemCraft(item);
        };
        GameObject craftText = ResourcesLoader.Load<GameObject>(GameData.Tags.CraftText);
        GameObject craftTextClone = Instantiate(craftText);
        SetUpCraftText(craftTextClone);
        item.bluePrintInstance.SetActive(true);
        yield return new WaitUntil(craftOrReturn);
        Destroy(craftTextClone);
    }

    public void CraftItemInDraft()
    {
        var draftInstance = InventoryDraft.GetInstance();
        ConstructedItem item = draftInstance.GetCraftableItemFromCraftingList();

            if(item is ConstructedStationaryItem)
            {
                StartCoroutine(ShowBluePrint((ConstructedStationaryItem)item));
            }
            else
            {
                //Debug.LogError("Unable to cast ConstructedItem into ConstructedStationaryItem, item"+item.tag);
                draftInstance.ConsolidateCraft();
            }

    }

}