using System;
using System.Collections;
using UnityEngine;
using ItemEnum = GameData.Enums.Items;
public class CookingProcess : MonoBehaviour
{
    public GameObject itemCookingRef;
    public Vector3 offset;

    // PickUpItem
    public Vector3 pickUpTextOffset;


    void Start()
    {
        itemCookingRef = null;
    }


    GameObject InstantiatePreparedPrefab(GameObject prefab, Quaternion rotation)
    {
        prefab.transform.position = transform.position + offset;
        prefab.transform.rotation = rotation;
        var clone = Instantiate(prefab);
        if(clone.GetComponent<PickUpItem>())
        {
            Destroy(clone.GetComponent<PickUpItem>());
        }
        return clone;
    }

    public void Cook(ItemEnum item, 
                     GameObject prefab, 
                     Quaternion rotation,
                     bool burnAfterTime, 
                     bool changeColor, 
                     Vector3 itemOffset,
                     Vector3 textOffset)
    {
        offset = itemOffset;
        pickUpTextOffset = textOffset;

        itemCookingRef = InstantiatePreparedPrefab(prefab, rotation);

        if (itemCookingRef.GetComponent<FishMovement>())
        {
            itemCookingRef.GetComponent<FishMovement>().enabled = false;
        }
        if (!Inventory.GetInstance().RemoveOneItem(item))
        {
            Debug.LogError("[CookFish] item not found in inventory: "+item);
            return;
        }
        StartCoroutine(CookProcess(burnAfterTime, changeColor));
        StartCoroutine(DestroyThisScript());

        return;
    }


    IEnumerator DestroyThisScript()
    {
        yield return new WaitUntil(() =>
        {
            return !itemCookingRef;
        });
        Destroy(gameObject.GetComponent<CookingProcess>());
    }

    IEnumerator CookProcess(bool burnAfterTime, bool changeColor)
    {
        yield return new WaitForSeconds(5);

        if(!gameObject.GetComponent<CampFire>().IsFireActive())
        {
            yield break;
        }

        ActionSwitcher switcher = gameObject.GetComponent<CampFire>().switcher;
        switcher.RemoveAllActionsExcept(gameObject.GetComponent<CampFire>().PUT_OUT);

        PickUpItemHelper pickUpHelper = new PickUpItemHelper(itemCookingRef);
        switcher.AddActionIfNotExist(PickUpItemHelper.PICK_UP, pickUpHelper.PickUpAction,
            () => switcher.RemoveActionIfExists(PickUpItemHelper.PICK_UP));

        itemCookingRef.GetComponent<CookConverter>().ConvertInCookedItem();

        if (changeColor)
        {
            itemCookingRef.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (burnAfterTime)
        {
            StartCoroutine(FishBurnedProcess());
        }
    }

    IEnumerator FishBurnedProcess()
    {
        yield return new WaitForSeconds(8);
        if (itemCookingRef)
        {
            gameObject.GetComponent<CampFire>().switcher.RemoveActionIfExists(PickUpItemHelper.PICK_UP);
            Destroy(itemCookingRef);
            itemCookingRef = null;
        }
    }

    public bool IsCooking()
    {
        return itemCookingRef;
    }



}
