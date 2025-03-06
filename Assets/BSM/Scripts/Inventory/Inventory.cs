using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : InventoryController
{

    public bool IsFull => _slots.Where(x => x._item != null).Count() >= _slots.Count;
    
    
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

    public void DropItem(int index)
    {
        _slots[index].RemoveItem();
    }
    
    public Item SelectedItem(int index)
    {
        if (_slots[index]._item != null)
        {
            return _slots[index]._item;
        }

        return null;
    }
    
}
