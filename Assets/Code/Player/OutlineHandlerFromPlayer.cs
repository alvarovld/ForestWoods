using UnityEngine;
using System.Collections;
using OutlineCamera;
using Utils;

public class OutlineHandlerFromPlayer : MonoBehaviour
{
    public int defaultColor;

    private void OnTriggerEnter(Collider item)
    {
        if(!ItemHelper.IsItem(item.tag))
        {
            return;
        }

        if(!item.gameObject.GetComponent<Outline>() &&
            item.gameObject.GetComponent<MeshRenderer>() != null)
        {
            item.gameObject.AddComponent<Outline>();
        }

        SetItemOutlineEnabled(true, item.gameObject);
        SetItemOutline(defaultColor, item.gameObject);
    }

    private void OnTriggerExit(Collider item)
    {
        if (!ItemHelper.IsItem(item.tag))
        {
            return;
        }

        SetItemOutlineEnabled(false, item.gameObject);
        RemoveOutline(item.gameObject);
    }



    void SetItemOutline(int color, GameObject item)
    {
        if (item.GetComponent<MeshRenderer>())
        {
            item.GetComponent<Outline>().ApplyOutline();
        }
        for (int i = 0; i < item.transform.childCount; ++i)
        {
            var child = item.transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().color = color;
                child.GetComponent<Outline>().ApplyOutline();
            }
        }
    }

    void SetItemOutlineEnabled(bool enabled, GameObject item)
    {
        if (item.GetComponent<Outline>() != null && item.GetComponent<Outline>().isActiveAndEnabled)
        {
            item.GetComponent<Outline>().enabled = enabled;
        }

        for (int i = 0; i < item.transform.childCount; ++i)
        {
            var child = item.transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().enabled = enabled;
            }
        }
    }

    void RemoveOutline(GameObject item)
    {
        if (item.GetComponent<Outline>() != null)
        {
            item.GetComponent<Outline>().RemoveOutline();
        }

        for (int i = 0; i < item.transform.childCount; ++i)
        {
            var child = item.transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().RemoveOutline();
            }
        }
    }

}
