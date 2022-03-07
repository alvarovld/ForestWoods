using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesLoader
{
    public static T Load<T>(string tag) where T : Object
    {
        // IMPORTANT: Add "/" at the end
        List<string> paths = new List<string>() { 
            "Prefabs/", 
            "Prefabs/Items/", 
            "Prefabs/Environment/", 
            "Prefabs/UI/", 
            "Prefabs/Enemies/", 
            "Icons/",
            "BluePrints/"
        };

        T resource = null;
        foreach (var path in paths)
        {
            resource = Resources.Load<T>(path+tag);
            if(resource)
            {
                break;
            }
        }

        return resource;
    }
}
