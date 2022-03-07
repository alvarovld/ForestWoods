using UnityEngine;
using System.Collections;
using Utils;
using System;

public class WaterActions : MonoBehaviour
{
    public Vector3 offset;
    public static string FILL_CAN = "Fill can";
    public static string DRINK = "Drink";

    void SetWaterActions()
    {
        ActionSwitcher switcher;
        if(gameObject.GetComponent<ActionSwitcher>())
        {
            switcher = gameObject.GetComponent<ActionSwitcher>();
            switcher.enabled = true;
        }
        else
        {
            switcher = gameObject.AddComponent<ActionSwitcher>();
            switcher.SetPositionProperties(GameObjectRefs.player, offset);
        }

        if(IsFillCanActionAvailable())
        {
            AddFillCanAction(switcher);
        }

        AddDrinkWaterAction(switcher);
    }


    void AddFillCanAction(ActionSwitcher switcher)
    {
        Action FillCan = () => {
            Inventory.GetInstance().RemoveOneItem(GameData.Enums.Items.EmptyCan);
            Inventory.GetInstance().AddOneItem(GameData.Enums.Items.FilledCan);
            GameObjectRefs.player.GetComponent<PlayerAnimatorHandler>().PlayPickUp();
        };

        switcher.AddActionIfNotExist(FILL_CAN, FillCan, () => switcher.RemoveActionIfExists(FILL_CAN));
    }

    void AddDrinkWaterAction(ActionSwitcher switcher)
    {
        Action DrinkDirtyWater = () => {
            PlayerStats.thirst = 100;
            PlayerStats.health -= UnityEngine.Random.Range(0, 15);
            GameObjectRefs.player.GetComponent<PlayerAnimatorHandler>().PlayPickUp();
        };
        switcher.AddActionIfNotExist(DRINK, DrinkDirtyWater);
    }

    bool IsFillCanActionAvailable()
    {
        return Inventory.GetInstance().Contains(GameData.Enums.Items.EmptyCan);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }

        SetWaterActions();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag.Equals(GameData.Tags.Player))
        {
            GetComponent<ActionSwitcher>().enabled = false;
        }
    }

}
