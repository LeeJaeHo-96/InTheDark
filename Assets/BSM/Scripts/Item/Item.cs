using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int _itemID;
    private InteractableItemData _itemData;

    private void OnEnable()
    {
        StartCoroutine(ItemSetDelayRoutine());
    }

    private void OnValidate()
    {
        if (_itemID <= 0)
        {
            Debug.LogError($"ID 값을 0이상 값으로 입력하세요. -{gameObject.name}-");
        }
    }

    private IEnumerator ItemSetDelayRoutine()
    {
        while (ItemManager.Instance == null)
        {
            yield return null; 
        }
        
        SetItemData(); 
    }
    
    /// <summary>
    /// 아이템 데이터 자동 할당
    /// </summary>
    private void SetItemData()
    {
        _itemData = ItemManager.Instance.GetItemData(_itemID);
    }

     
    public void ItemPosition(float x, float y, float z)
    {
        
    }
    
}
