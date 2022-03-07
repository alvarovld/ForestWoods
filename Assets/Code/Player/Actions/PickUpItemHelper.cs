using UnityEngine;
using System.Collections;
using Utils;

public class PickUpItemHelper 
{
    PlayerAnimatorHandler animator;
    public static string PICK_UP = "Pick up";
    GameObject item;

    public PickUpItemHelper(GameObject item)
    {
        this.item = item;
        //animator = GameObjectRefs.player.GetComponent<PlayerAnimatorHandler>();
    }


    public void PickUpAction()
    {
        Debug.Log("[PickUpObject] Object picked up: " + item.tag);
        Inventory.GetInstance().AddOneItem(GameData.Converters.TagToItem.Get(item.tag));
        //animator.PlayPickUp();
        ObjectPoolManager.GetInstance().ReturnObjectToPoolDisabling(item);
    }

    public void CreatePickUpActionIfNotExists(ActionSwitcher switcher)
    {
        if (!switcher)
        {
            Debug.LogError("[PickUpItemHelper] Switcher not set");
            return;
        }

        switcher.enabled = true;
        switcher.executeOnlyOnce = true;
        switcher.AddActionIfNotExist(PICK_UP, PickUpAction, ()=> switcher.RemoveActionIfExists(PICK_UP));
    }
}
