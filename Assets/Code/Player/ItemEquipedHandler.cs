using GameData.Converters;
using UnityEngine;
using UnityEngine.SceneManagement;
using ItemEnum = GameData.Enums.Items;
public class ItemEquipedHandler : MonoBehaviour
{
    static ItemEquipedHandler instance;
    GameObject flashlightObj;
    GameObject fireTorchObj;
    ItemEnum currentItem;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
            return;
        }

        currentItem = ItemEnum.NullItem;
        flashlightObj = Utils.GameObjectRefs.GetChildByTag(gameObject, GameData.Tags.FlashLight).gameObject;
        fireTorchObj = Utils.GameObjectRefs.GetChildByTag(gameObject, GameData.Tags.FireTorch).gameObject;
        InputEventManager.GetInstance().SetNewEvent(HandleLightOnOff, GameData.Keys.ONOFF_LIGHT, false);
    }

    public static ItemEquipedHandler GetInstance()
    {
        return instance;
    }

    public void SelectItem(string tag)
    {
        SelectItem(TagToItem.Get(tag));
    }

    void DrinkBoiledWater()
    {
        PlayerStats.thirst = 100;
        Inventory.GetInstance().RemoveOneItem(ItemEnum.FilledBoiledCan);
        Inventory.GetInstance().AddOneItem(ItemEnum.EmptyCan);
    }

    public void SelectItem(ItemEnum item)
    {
        UnequipCurrentItem();
        currentItem = item;

        switch(currentItem)
        {
            case ItemEnum.Flashlight:
                TurnLightOn();
                InputHandler.GetInstance().CloseInventory();
                break;
            case ItemEnum.FireTorch:
                TurnLightOn();
                InputHandler.GetInstance().CloseInventory();
                break;
            case ItemEnum.CookedFish:
                PlayerStats.Feed(currentItem);
                Inventory.GetInstance().RemoveOneItem(currentItem);
                break;
            case ItemEnum.Blueberry:
                PlayerStats.Feed(currentItem);
                Inventory.GetInstance().RemoveOneItem(currentItem);
                break;
            case ItemEnum.FilledCan:
                DrinkDirtyWater();
                break;
            case ItemEnum.FilledBoiledCan:
                DrinkBoiledWater();
                break;
            default:
                Debug.LogWarning("[ItemHandler] trying to equip item not implemented");
                break;
        }
    }

    public void DrinkDirtyWater()
    {
        Inventory.GetInstance().EmptyCan();
        PlayerStats.thirst = 100;
        PlayerStats.health -= Random.Range(0, 15);
    }

    void UnequipItem(ItemEnum item)
    {
        switch (item)
        {
            case ItemEnum.Flashlight:
                flashlightObj.SetActive(false);
                break;
            case ItemEnum.FireTorch:
                fireTorchObj.SetActive(false);
                break;
            default:
                Debug.LogWarning("[ItemHandler] trying to unequip unknown item: " + item);
                break;
        }
        currentItem = ItemEnum.NullItem;
    }

    public void UnequipCurrentItem()
    {
        UnequipItem(currentItem);
    }

    public ItemEnum GetEquipedItem()
    {
        return currentItem;
    }

    public void TurnLightOff()
    {
        PlayerInfo.lightOn = false;
        switch (currentItem)
        {
            case ItemEnum.Flashlight:
                TurnOffFlashlight();
                break;
            case ItemEnum.FireTorch:
                TurnOffFireTorchLight();
                break;
            default:
                Debug.LogWarning("[ItemHandler] [turnLightOff] No light selected");
                break;
        }
    }

    public void TurnLightOn()
    {
        PlayerInfo.lightOn = true;
        switch (currentItem)
        {
            case ItemEnum.Flashlight:
                TurnOnFlashlight();
                break;
            case ItemEnum.FireTorch:
                TurnOnFireTorchLight();
                break;

            default:
                Debug.LogWarning("[ItemHandler] [turnLightOn] No light selected");
                break;
        }
    }

    void TurnOnFireTorchLight()
    {
        fireTorchObj.SetActive(true);
        fireTorchObj.GetComponent<FireTorchHandler>().TurnOn();
    }

    void TurnOffFireTorchLight()
    {
        fireTorchObj.GetComponent<FireTorchHandler>().TurnOff();
    }

    void TurnOnFlashlight()
    {
        flashlightObj.SetActive(true);
        flashlightObj.GetComponent<Light>().enabled = true;
    }

    void TurnOffFlashlight()
    {
        flashlightObj.GetComponent<Light>().enabled = false;
    }

    void HandleLightOnOff()
    {
        if(PlayerInfo.lightOn)
        {
            TurnLightOff();
        }
        else
        {
            TurnLightOn();
        }
    }
}
