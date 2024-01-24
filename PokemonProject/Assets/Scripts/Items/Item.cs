using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemSO so;
    public int count;

    public void IncrementCount()
    {
        count++;
    }

    public void DecrementCount()
    {
        if (count > 0)
        {
            count--;
        }
    }

    public bool CheckCount()
    {
        if (count > 0)
        {
            return true;
        }

        return false;
    }
    
}
