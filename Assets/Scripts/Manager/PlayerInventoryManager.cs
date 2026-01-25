using System.Collections.Generic;
using UnityEngine;


public class PlayerInventoryManager : MonoBehaviour
{
    private HashSet<string> items = new HashSet<string>();
    public void Add(string itemName)
    {
        if (itemName == "") return;
        if (!Has(itemName))
        {
            items.Add(itemName);
        }
    }
    public bool Has(string itemName)
    {
        return items.Contains(itemName);
    }
}