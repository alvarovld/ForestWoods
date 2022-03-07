using UnityEngine;
using Utils;

public class DisableOrEnableObjectsInPool : MonoBehaviour
{
    public float distanceToDisable;
    Transform playerTransform;
    public GameObject objectPool;

    private void Start()
    {
        playerTransform = GameObjectRefs.player.transform;
    }

    bool HasToDisable(Vector3 position)
    {
        var point = GameObjectRefs.camera.WorldToViewportPoint(position);
        return !CameraView.PositionInCameraView(position) ||
               DistanceToPlayer(position) > distanceToDisable;
    }

    float DistanceToPlayer(Vector3 position)
    {
        return (playerTransform.position - position).magnitude;
    }


    void DisableOrEnable()
    {
        for (int i = 0; i < objectPool.transform.childCount; ++i)
        {
            var item = objectPool.transform.GetChild(i);

            if(!item || IsCanvasObject(item))
            {
                continue;
            }
            if (HasToDisable(item.transform.position))
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
    }


    bool IsCanvasObject(Transform item)
    {
        if(item.GetComponent<Canvas>())
        {
            return true;
        }
        return false;
    }


    private void Update()
    {
        DisableOrEnable();
    }

}
