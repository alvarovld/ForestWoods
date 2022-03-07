using UnityEngine;
using System.Collections;
using Utils;
using System;
using System.Collections.Generic;

public class PickUpItemFromPlayer : MonoBehaviour
{
    Vector3 offset;
    public float distanceToPickUp = 0;

    Dictionary<string, ActionSwitcher> switcherDic = new Dictionary<string, ActionSwitcher>();

    PickUpItemHelper pickUpHelper;

    private void Start()
    {
        offset = GameData.Parameters.itemtextOffset;
        tag = gameObject.tag;
        if (distanceToPickUp == 0)
        {
            distanceToPickUp = 20 / gameObject.transform.localScale.x;
        }
        else
        {
            distanceToPickUp = distanceToPickUp / gameObject.transform.localScale.x;
        }
    }

    bool IsPlayerCloseEnough(Transform item)
    {
        return (transform.position - item.position).sqrMagnitude < distanceToPickUp * distanceToPickUp;
    }

    private void OnTriggerStay(Collider item)
    {
        string tag = item.gameObject.tag;
        if (!ItemHelper.IsItem(tag))
        {
            return;
        }
        if (!IsPlayerCloseEnough(item.transform))
        {
            if (switcherDic.ContainsKey(tag))
            {
                switcherDic[tag].enabled = false;
            }
            return;
        }

        if (!switcherDic.ContainsKey(tag))
        {
            switcherDic.Add(tag, gameObject.AddComponent<ActionSwitcher>());
        }

        ActionSwitcher switcher = switcherDic[tag];
        pickUpHelper = new PickUpItemHelper(item.gameObject);
        switcher.SetPositionProperties(item.transform, offset);

        if (InventoryCapacity.HasReachedItemLimit(item.gameObject.tag))
        {
            switcher.enabled = false;
            return;
        }

        pickUpHelper.CreatePickUpActionIfNotExists(switcher);
    }


    private void OnTriggerExit(Collider other)
    {
        if (!ItemHelper.IsItem(other.gameObject.tag) || !switcherDic.ContainsKey(other.gameObject.tag))
        {
            return;
        }
        switcherDic[other.gameObject.tag].RemoveAllActions();
        switcherDic[other.gameObject.tag].enabled = false;
    }
}
