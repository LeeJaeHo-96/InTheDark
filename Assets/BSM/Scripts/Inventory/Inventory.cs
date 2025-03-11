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
    
    /// <summary>
    /// 주운 아이템 슬롯에 추가
    /// </summary>
    /// <param name="item"></param>
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

    /// <summary>
    /// 인벤토리에 들어있는 아이템 드랍
    /// </summary>
    /// <param name="index"></param>
    public void DropItem(int index)
    {
        _slots[index].RemoveItem();
    }
    
    /// <summary>
    /// 아이템 선택
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item SelectedItem(int index)
    {
        if (_slots[index]._item != null)
        {
            return _slots[index]._item;
        }

        return null;
    }
    
}
