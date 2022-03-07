using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using ItemEnum = GameData.Enums.Items;
public class IngredientsUI : MonoBehaviour
{
    public void SetItemData(ConstructedItem item)
    {
        SetName(item.tag);
        SetIngredients(item.recipe.GetDictionary());
    }

    void SetName(string name)
    {
        transform.GetChild(0).GetComponent<Text>().text = name+":";
    }

    void FillIngredient(Transform ui, string iconName, int amount)
    {
        ui.GetChild(0).GetComponent<Image>().sprite = ResourcesLoader.Load<Sprite>(iconName);
        ui.GetChild(1).GetComponent<Text>().text = "  x " + amount;
    }

    void SetIngredients(Dictionary<ItemEnum, int> ingredients)
    {
        foreach(var key in ingredients.Keys)
        {
            int amount = ingredients[key];
            
            GameObject ingredient = Instantiate(ResourcesLoader.Load<GameObject>(GameData.Tags.IngredientsUI));
            ingredient.transform.SetParent(transform);
            FillIngredient(ingredient.transform, key.ToString() + "Icon", amount);
        }
    }

}
