using OutlineCamera;
using UnityEngine;
using Utils;

public class OutlineHandler : MonoBehaviour
{
    public OutlineData data;


    private void Awake()
    {
        if(!gameObject.GetComponent<Outline>() && gameObject.GetComponent<MeshRenderer>() != null)
        {
            gameObject.AddComponent<Outline>();
        }

        if(!gameObject.GetComponent<SphereCollider>())
        {
            AddSphereCollider();
        }

        SetOutlineEnabled(false);
    }

    void AddSphereCollider()
    {
        var sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = data.distanceToShine / gameObject.transform.localScale.x;
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            SetOutlineEnabled(true);
            SetOutline(data.defaultColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(GameData.Tags.Player))
        {
            SetOutlineEnabled(false);
            RemoveOutline();
        }
    }



    void SetOutline(int color)
    {
        if(gameObject.GetComponent<MeshRenderer>())
        {
            gameObject.GetComponent<Outline>().ApplyOutline();
        }
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().color = color;
                child.GetComponent<Outline>().ApplyOutline();
            }
        }
    }

    private void OnDisable()
    {
        RemoveOutline();
    }

    void SetOutlineEnabled(bool enabled)
    {
        if (gameObject.GetComponent<Outline>() != null && gameObject.GetComponent<Outline>().isActiveAndEnabled)
        {
            gameObject.GetComponent<Outline>().enabled = enabled;
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().enabled = enabled;
            }
        }
    }

    void RemoveOutline()
    {
        if (gameObject.GetComponent<Outline>() != null)
        {
            gameObject.GetComponent<Outline>().RemoveOutline();
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Outline>() != null)
            {
                child.GetComponent<Outline>().RemoveOutline();
            }
        }
    }


}
