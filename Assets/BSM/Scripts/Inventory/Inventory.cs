using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : InventoryController
{

    void Awake()
    {
        base.Awake();
    }
    
    public void GetItem(Item item)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i]._item == null)
            {
                _slots[i].AddItem(item);
                return;
            } 
        } 
    }
    
}
