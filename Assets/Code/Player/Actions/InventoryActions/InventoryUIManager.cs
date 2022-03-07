using GameData;
using UnityEngine;
using UnityEngine.UI;
using ItemEnum = GameData.Enums.Items;

public class InventoryUIManager : MonoBehaviour
{
    public Transform itemParent;
    public Button craftButton;
    public GameObject itemText;
    public GameObject availableRecipestext;
    InventoryDraft inventoryDraftInstance;

    private void OnEnable()
    {
        inventoryDraftInstance = InventoryDraft.GetInstance();
        inventoryDraftInstance.Reset();
    }

    private void Start()
    {
        itemText.transform.GetChild(0).GetComponent<Text>().color = Parameters.textColor;
        ResetUI();
    }

    public void ResetUI()
    {
        inventoryDraftInstance.Reset();
        itemText.SetActive(false);
        ResetAvailableRecipesUI();
        EnableChildItemsBasedOnInventory.Execute(itemParent);
    }

    public void ResetAvailableRecipesUI()
    {
        for(int i = 0; i < availableRecipestext.transform.GetChild(0).childCount; ++i)
        {
            Destroy(availableRecipestext.transform.GetChild(0).GetChild(i).gameObject);
        }
        availableRecipestext.SetActive(false);
    }


    void ShowCraftButtonWhenRecipeIsValid()
    {
        if(InventoryDraft.GetInstance().isCraftingListEmpty())
        {
            craftButton.gameObject.SetActive(false);
            return;
        }

        Item item = InventoryDraft.GetInstance().GetCraftableItemFromCraftingList();

        if(item.itemEnum.Equals(ItemEnum.NullItem))
        {
            craftButton.gameObject.SetActive(false);
        }
        else
        {
            availableRecipestext.SetActive(false);
            craftButton.gameObject.SetActive(true);
            craftButton.GetComponentInChildren<Text>().text = item.tag;
            return;
        }
    }

    // Craft button listener
    public void CraftAction()
    {
        CraftManager.GetInstance().CraftItemInDraft();
        EnableChildItemsBasedOnInventory.Reset(itemParent);
        craftButton.gameObject.SetActive(false);
    }


    private void Update()
    {
        ShowCraftButtonWhenRecipeIsValid();
    }

}
