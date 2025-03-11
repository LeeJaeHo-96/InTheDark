using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FlashLight : Item
{
    private float _battery;
    
    private bool isPower;
     
    
    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("자식 OnEnable");
        _battery = _itemData.MaxDurability;
    }

    public override void ItemUse()
    {
        Debug.Log("손전등 사용");
    }
    
}
