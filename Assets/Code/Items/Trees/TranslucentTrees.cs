using UnityEngine;
using Utils;

public class TranslucentTrees : MonoBehaviour
{
    Material translucent;
    Material[] translucentArray;
    Material[] defaultMaterials;
    MeshRenderer rend;

    GameObject currentTree = null;

    private void Awake()
    {
        translucentArray = new Material[4];
        rend = Resources.Load<MeshRenderer>("ConniferMesh");
        translucent = Resources.Load<Material>("Mats/Transparent");
        defaultMaterials = rend.sharedMaterials;
        FillTranslucentArray();
    }

    void FillTranslucentArray()
    {
        for (int i = 0; i < translucentArray.Length; ++i)
        {
            translucentArray[i] = translucent;
        }
    }

    RaycastHit[] GetRaycastedObjects()
    {
        var camera = GameObjectRefs.camera.transform;
        Ray ray = new Ray(camera.position, transform.position - camera.position);
        return Physics.RaycastAll(ray, (transform.position - camera.position).magnitude);
    }

    void ChangeMaterialsToTranslucent(MeshRenderer rend)
    {
        rend.materials = translucentArray;
    }

    void ResetMaterials(MeshRenderer rend)
    {
        rend.materials = defaultMaterials;
    }

    bool CurrentTreeIsInRaycastedList(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            var parent = hit.collider.gameObject.transform.parent;
            if (parent == null)
            {
                continue;
            }
            if (parent.gameObject.Equals(currentTree))
            {
                return true;
            }
        }
        return false;

    }

    void DisableCurrentTreeWhenNotInCameraView(RaycastHit[] hits)
    {
        if(CurrentTreeIsInRaycastedList(hits))
        {
            return;
        }

        ResetMaterials(currentTree.GetComponentInChildren<MeshRenderer>());
    }

    void ChangeRendererOfTree(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            var parent = hit.collider.gameObject.transform.parent;
            if (parent == null)
            {
                continue;
            }

            if (parent.gameObject.tag.Equals(GameData.Tags.Tree))
            {
                currentTree = parent.gameObject;
                currentTree.GetComponentInChildren<MeshRenderer>().materials = translucentArray;
                return;
            }
        }

    }

    private void FixedUpdate()
    {
        var hits = GetRaycastedObjects();
        ChangeRendererOfTree(hits);
        if (currentTree != null)
        {
            DisableCurrentTreeWhenNotInCameraView(hits);
        }
    }

}
