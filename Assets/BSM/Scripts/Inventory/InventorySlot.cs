using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Image _slotItemImage; 
    public Item _item;


    private void Awake()
    {
        _slotItemImage = GetComponent<Image>();
    }

    public void AddItem(Item item)
    {
        _item = item;
        _slotItemImage.sprite = _item.GetItemImage();
    }

    public void RemoveItem()
    {
        if (_item == null) return;

        _item = null;
        _slotItemImage.sprite = null;

    }
    
}
