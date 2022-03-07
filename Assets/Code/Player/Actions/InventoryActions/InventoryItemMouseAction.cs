using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Outline = OutlineCamera.Outline;

public class InventoryItemMouseAction : MonoBehaviour
{
    public float distanceFromBeginning;
    public Transform craftingBoard;
    public InventoryUIManager manager;
    public Canvas canvas;
    Vector3 textOffset;

    public Vector3 initialPosition;
    Vector3 craftPosition;
    public float height;
    private void Awake()
    {
        textOffset = new Vector3(0, 25, 0);
        initialPosition = transform.position;
        craftPosition = initialPosition + distanceFromBeginning * Direction();
        craftPosition.y = height;
        canvas.transform.GetChild(0).GetComponent<Text>().color = GameData.Parameters.textColor;
    }

    private void OnEnable()
    {
        StartCoroutine(ResetPosition());
    }

    Vector3 Direction()
    {
        var axis = transform.parent ? transform.parent.position : transform.position;
        return (craftingBoard.position - axis).normalized;
    }

    bool IsInCraftingList()
    {
        return transform.position == craftPosition;
    }

    void DisableOutline()
    {
        if (gameObject.GetComponent<Outline>() != null)
        {
            gameObject.GetComponent<Outline>().RemoveOutline();
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().RemoveOutline();
            }
        }
    }

    void SetOutline(int color)
    {
        if (gameObject.GetComponent<MeshRenderer>())
        {
            gameObject.GetComponent<Outline>().ApplyOutline();
        }
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().ApplyOutline();
                child.GetComponent<Outline>().color = color;
            }
        }
    }


    void ShowCraftableRecipesUI()
    {
        manager.ResetAvailableRecipesUI();
        manager.availableRecipestext.SetActive(true);
        List<ConstructedItem> items = ItemsSheet.GetCraftableItemsFromCraftingList();

        foreach(var item in items)
        {
            var recipeUI = Instantiate(ResourcesLoader.Load<GameObject>(GameData.Tags.AvailableRecipeUI));
            recipeUI.transform.SetParent(manager.availableRecipestext.transform.GetChild(0));
            recipeUI.GetComponent<IngredientsUI>().SetItemData(item);
        }
    }

    void HideCraftableRecipesText()
    {
        manager.availableRecipestext.SetActive(false);
    }

    void HandleCraftableRecipesText()
    {
        if(InventoryDraft.GetInstance().GetCraftingList().GetDictionary().Count == 0)
        {
            manager.ResetAvailableRecipesUI();
            return;
        }

        if (ItemsSheet.GetCraftableItemsFromCraftingList().Count > 0)
        {
            ShowCraftableRecipesUI();
        }
        else
        {
            manager.ResetAvailableRecipesUI();
        }
    }

    void HandleItemMovement()
    {
        if (!IsInCraftingList())
        {
            InventoryDraft.GetInstance().AddItemToCraftingList(transform.tag);
            transform.position = craftPosition;
        }
        else if (IsInCraftingList())
        {
            InventoryDraft.GetInstance().RemoveItemFromCraftingList(transform.tag);
            transform.position = initialPosition;
        }
    }

    void HandleEquipItem()
    {
        ItemEquipedHandler.GetInstance().SelectItem(gameObject.tag);
        manager.ResetUI();
    }

    void HideItemText()
    {
        canvas.gameObject.SetActive(false);
    }

    void SetItemText(string text)
    {
        canvas.transform.GetChild(0).GetComponent<Text>().text = text;
    }

    void ShowItemText()
    {
        canvas.gameObject.SetActive(true);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.gameObject.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        canvas.transform.GetChild(0).position = canvas.transform.TransformPoint(pos) + textOffset;

        if (transform.position != initialPosition)
        {
            SetItemText("Remove (right click)");
        }
        else if(transform.gameObject.tag.Equals(GameData.Tags.CookedFish) ||
                transform.gameObject.tag.Equals(GameData.Tags.BlueBerry))
        {
            SetItemText("Eat (left click) \nCraft (right click)");
        }
        else if(transform.gameObject.tag.Equals(GameData.Tags.FilledCan))
        {
            SetItemText("Drink (left click) \nCraft (right click)");
        }
        else
        {
            SetItemText("Equip (left click) \nCraft (right click)");
        }

    }

    private void OnMouseOver()
    {
        SetOutline(0);

        ShowItemText();

        if (Input.GetMouseButtonDown(1))
        {
            HandleItemMovement();
            HandleCraftableRecipesText();
        }
        else if(Input.GetMouseButtonDown(0) && !IsInCraftingList())
        {
            HandleEquipItem();
        }
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitUntil(() =>
        {
            return Input.GetKeyDown(GameData.Keys.RESET_INVENTORY_ITEMS);
        });

        if (IsInCraftingList())
        {
            InventoryDraft.GetInstance().RemoveItemFromCraftingList(transform.tag);
        }

        transform.position = initialPosition;
        manager.ResetAvailableRecipesUI();
        StartCoroutine(ResetPosition());

    }


    void RemoveOutline()
    {

        if (GetComponent<Outline>() != null)
        {
            GetComponent<Outline>().enabled = false;
        }
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void OnMouseExit()
    {
        DisableOutline();
        HideItemText();
    }
}
