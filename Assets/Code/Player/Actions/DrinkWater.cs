using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class DrinkWater : MonoBehaviour
{
    public Text drinkText;
    public Vector3 offset;

    private void Start()
    {
        var drinkObj = Instantiate(ResourcesLoader.Load<GameObject>(GameData.Tags.DrinkText));
        drinkText = drinkObj.transform.GetChild(0).GetComponent<Text>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.gameObject.tag.Equals(GameData.Tags.Player) || PlayerStats.thirst == 100)
        {
            return;
        }


        ShowText();
        InputEventManager.GetInstance().SetNewEvent(Drink, GameData.Keys.ACTION, true);
    }

    private void OnTriggerExit(Collider other)
    {
        HideText();
        InputEventManager.GetInstance().RemoveEvent(Drink, GameData.Keys.ACTION, true);
    }


    void ShowText()
    {
        drinkText.transform.position = GameObjectRefs.camera.WorldToScreenPoint(GameObjectRefs.player.position + offset);
        drinkText.gameObject.SetActive(true);
        drinkText.text = "Drink ("+ GameData.Keys.ACTION + ")";
    }

    void HideText()
    {
        drinkText.gameObject.SetActive(false);
    }


    void Drink()
    {
        PlayerStats.thirst = 100;
        GameObjectRefs.player.GetComponent<PlayerAnimatorHandler>().PlayPickUp();
        HideText();
    }
}
