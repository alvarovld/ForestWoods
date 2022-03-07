using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfChilds : MonoBehaviour
{
    public int numberOfChilds;
    public int rags;
    public int stones;
    public int sticks;
    public int maxNumber = 0;
    void LateUpdate()
    {
        numberOfChilds = transform.childCount;
        if(numberOfChilds > maxNumber)
        {
            maxNumber = numberOfChilds;
        }
    }

    int getChilgCount(string tag)
    {
        int count = 0;
        for(int i = 0; i < transform.childCount; ++i)
        {
            if(transform.GetChild(i).gameObject.tag.Equals(tag))
            {
                ++count;
            }
        }
        return count;
    }

}
