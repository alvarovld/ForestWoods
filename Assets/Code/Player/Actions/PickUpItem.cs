using UnityEngine;
using Utils;

// TODO: DELETE THIS, DEPRECATED
public class PickUpItem : MonoBehaviour
{
    public Vector3 offset;
    public float distanceToPickUp = 0;
    ActionSwitcher switcher;
    PickUpItemHelper pickUpHelper;
    new string tag;

    private void Start()
    {
        offset = GameData.Parameters.itemtextOffset;
        tag = gameObject.tag;
        pickUpHelper = new PickUpItemHelper(gameObject);
        if (distanceToPickUp == 0)
        {
            distanceToPickUp = 20 / gameObject.transform.localScale.x;
        }
    }

    bool IsPlayerCloseEnough()
    {
        return (GameObjectRefs.player.position - gameObject.transform.position).sqrMagnitude < distanceToPickUp * distanceToPickUp;
    }    

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            return;
        }
        if (!IsPlayerCloseEnough())
        {
            if(switcher != null)
            {
                switcher.enabled = false;
            }
            return;
        }

        if (switcher == null)
        {
            switcher = gameObject.AddComponent<ActionSwitcher>();
            switcher.SetPositionProperties(transform, offset);
        }

        if (InventoryCapacity.HasReachedItemLimit(tag))
        {
            switcher.enabled = false;
            return;
        }

        pickUpHelper.CreatePickUpActionIfNotExists(switcher);
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals(GameData.Tags.Player) && switcher)
        {
            switcher.enabled = false;
        }
    }
}
