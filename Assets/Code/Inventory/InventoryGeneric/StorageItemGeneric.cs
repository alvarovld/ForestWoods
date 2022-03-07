using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageItemGeneric<T> 
{
    public T id;
    public int capacity;
    public ObjectAmountList<T> recipe;
    public float bluePrintOffset;
    public string bluePrintTag;

    public StorageItemGeneric(T id, int capacity, ObjectAmountList<T> recipe, float bluePrintOffset, string bluePrintTag)
    {
        this.id = id;
        this.capacity = capacity;
        this.recipe = recipe;
        this.bluePrintOffset = bluePrintOffset;
        this.bluePrintTag = bluePrintTag;
    }

    public StorageItemGeneric(T id, int capacity)
    {
        this.id = id;
        this.capacity = capacity;
    }
}

