using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using ItemEnum = GameData.Enums.Items;

public class CookManager : MonoBehaviour
{
    ActionSwitcher switcher;
    static string COOK_FISH = "Cook fish";
    static string BOIL_WATER = "Boil water";
    public float foodOffset;

    private void Start()
    {
        switcher = gameObject.GetComponent<CampFire>().switcher;
    }


    public void AddPickUpActionToItemInCampfireIfExist()
    {
        if(gameObject.GetComponent<CookingProcess>() && gameObject.GetComponent<CookingProcess>().itemCookingRef)
        {
            PickUpItemHelper pickUpHelper = new PickUpItemHelper(gameObject.GetComponent<CookingProcess>().itemCookingRef);
            switcher.AddActionIfNotExist(PickUpItemHelper.PICK_UP, pickUpHelper.PickUpAction, () => switcher.RemoveActionIfExists(PickUpItemHelper.PICK_UP));
        }    
    }


    void AddCookFishOptionIfNotExists()
    {
        Action action = () => {
            GetComponent<CookingProcess>().Cook(ItemEnum.Fish,
                                        ResourcesLoader.Load<GameObject>(GameData.Tags.Fish),
                                        Quaternion.Euler(-90, 0, 0),
                                        true, 
                                        true,
                                        Vector3.up * foodOffset,
                                        GameData.Parameters.itemtextOffset);
        };

        switcher.AddActionIfNotExist(COOK_FISH, action, RemoveCookFishOptionWhenRequired);
    }

    void AddBoilWaterOptionIfNotExists()
    {
        Action action = () => {
            GetComponent<CookingProcess>().Cook(ItemEnum.FilledCan,
                                        ResourcesLoader.Load<GameObject>(GameData.Tags.FilledCan),
                                        Quaternion.identity,
                                        false,
                                        false,
                                        Vector3.up * foodOffset,
                                        GameData.Parameters.itemtextOffset);
        };

        switcher.AddActionIfNotExist(BOIL_WATER, action, () => switcher.RemoveActionIfExists(BOIL_WATER));
    }

    bool IsFireActive()
    {
        return transform.GetChild(0).gameObject.activeSelf;
    }

    bool HasFishesInInventory()
    {
        return Inventory.GetInstance().GetCount(ItemEnum.Fish) > 0;
    }


    bool IsAbleToCookFish()
    {
        if (!HasFishesInInventory() ||
            InventoryCapacity.HasReachedItemLimit(GameData.Tags.CookedFish))
        {
            return false;
        }

        if (GetComponent<CookingProcess>().IsCooking() || !IsFireActive())
        {
            return false;
        }

        return true;
    }

    bool IsAbleToBoilWater()
    {
        if (!Inventory.GetInstance().Contains(ItemEnum.FilledCan))
        {
            return false;
        }
        if (GetComponent<CookingProcess>().IsCooking() || !IsFireActive())
        {
            return false;
        }
        return true;
    }

    void RemoveCookFishOptionWhenRequired()
    {
        if(!IsAbleToCookFish())
        {
            switcher.RemoveActionIfExists(COOK_FISH);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }

        ProcessCookActions();
    }

    public void ProcessCookActions()
    {
        switcher.enabled = true;
        if (!GetComponent<CookingProcess>())
        {
            var cookUi = gameObject.AddComponent<CookingProcess>();
        }

        if (IsAbleToCookFish())
        {
            AddCookFishOptionIfNotExists();
        }
        else
        {
            switcher.RemoveActionIfExists(COOK_FISH);
        }

        if (IsAbleToBoilWater())
        {
            AddBoilWaterOptionIfNotExists();
        }
        else
        {
            switcher.RemoveActionIfExists(BOIL_WATER);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }
        switcher.enabled = false;
    }


}
