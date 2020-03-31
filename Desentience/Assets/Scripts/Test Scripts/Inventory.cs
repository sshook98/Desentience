using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int capacity;

    public bool AddItem(Item item)
    {
        if (items.Count >= capacity)
        {
            return false;
        } else
        {
            items.Add(item);
            return true;
        }
    }

    public bool RemoveItem(Item item)
    {
        return items.Remove(item);
    }
    
}
