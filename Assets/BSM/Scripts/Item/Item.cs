using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Item : MonoBehaviourPun
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

     
    public void PickUp(Player player, Transform armTransform)
    {
        photonView.TransferOwnership(player);
        transform.SetParent(armTransform);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void Drop(Transform dropTransform, Player player = null)
    {
        Vector3 dropPos = new Vector3(dropTransform.position.x, 1, dropTransform.position.z);
        
        photonView.TransferOwnership(player);
        transform.parent = null;
        transform.SetLocalPositionAndRotation(dropPos, Quaternion.identity);
    }
    
}
