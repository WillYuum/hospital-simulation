using System.Collections.Generic;
using UnityEngine;

public class GInventory
{
    List<GameObject> items = new();

    public void AddItem(GameObject i)
    {
        items.Add(i);
    }

    public void RemoveItem(GameObject t)
    {
        int index = -1;
        foreach (GameObject i in items)
        {
            index++;
            if (i == t)
            {
                break;
            }
        }

        if (index != -1)
        {
            items.RemoveAt(index);
        }
    }

    public GameObject FindItemWithTag(string t)
    {
        foreach (GameObject g in items)
        {
            if (g.CompareTag(t))
            {
                return g;
            }
        }
        return null;
    }

}
