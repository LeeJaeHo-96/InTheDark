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

    /// <summary>
    /// 아이템 추가
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        _item = item;
        _slotItemImage.sprite = _item.GetItemImage();
        _slotItemImage.color = new Color(1f, 1f, 1f, 1f);
    }

    /// <summary>
    /// 아이템 제거
    /// </summary>
    public void RemoveItem()
    {
        if (_item == null) return;

        _item = null;
        _slotItemImage.sprite = null;
        _slotItemImage.color = new Color(1f, 1f, 1f, 0f);
    }
    
}
