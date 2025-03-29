using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] protected GameObject _slotParent;
    
    protected List<InventorySlot> _slots = new List<InventorySlot>();

    protected void Awake()
    {
        for(int i = 0; i < _slotParent.transform.childCount; i++)
        {
            _slots.Add(_slotParent.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        } 
    }
}
