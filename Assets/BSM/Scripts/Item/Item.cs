using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PhotonTransformView), typeof(PhotonView))]
public class Item : MonoBehaviourPun
{
    [SerializeField] private int _itemID;
    protected InteractableItemData _itemData;
    private Rigidbody _itemRb;
    public bool IsOwned;
    private float _itemWeight;
    
    protected void Awake()
    {
        _itemRb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        Debug.Log("아이템 부모 enable");
        StartCoroutine(ItemSetDelayRoutine());
        
    }
    
    private void OnValidate()
    {
        if (_itemID <= 0)
        {
            Debug.LogError($"ID 값을 0이상 값으로 입력하세요. -{gameObject.name}-");
        }
    }
    
    /// <summary>
    /// 아이템 데이터 셋팅 딜레이 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator ItemSetDelayRoutine()
    {
        while (ItemManager.Instance == null)
        {
            yield return null; 
        }
        
        SetItemData();
        ItemRestore();
    }
    
    /// <summary>
    /// 아이템 데이터 자동 할당
    /// </summary>
    private void SetItemData()
    {
        _itemData = ItemManager.Instance.GetItemData(_itemID);
        _itemWeight = _itemData.ItemWeight;
    }

    protected virtual void ItemRestore()
    {
        
    }
    
    /// <summary>
    /// 아이템 주운 후 동작
    /// </summary>
    /// <param name="player"></param>
    public void PickUp(Player player)
    {
        _itemRb.isKinematic = true;
        photonView.TransferOwnership(player);
        photonView.RPC(nameof(SyncOwnershipRPC), RpcTarget.AllViaServer, true);
    }

    /// <summary>
    /// 아이템 버린 후 동작
    /// </summary>
    /// <param name="player"></param>
    public void Drop(Player player = null)
    {
        _itemRb.isKinematic = false;
        photonView.TransferOwnership(null);
        photonView.RPC(nameof(SyncOwnershipRPC), RpcTarget.AllViaServer, false);
    }
    
    /// <summary>
    /// 소유권 동기화
    /// </summary>
    /// <param name="isOwner"></param>
    [PunRPC]
    protected void SyncOwnershipRPC(bool isOwner = false)
    {
        IsOwned = isOwner;
        _itemRb.isKinematic = isOwner;
    }

    /// <summary>
    /// 현재 들고 있는 아이템의 무게 반환
    /// </summary>
    /// <returns></returns>
    public float GetItemWeight()
    {
        return _itemWeight;
    }

    /// <summary>
    /// 아이템 타입
    /// </summary>
    /// <returns></returns>
    public ItemHoldingType GetHoldingType()
    {
        return _itemData.ItemHoldingType;
    }

    /// <summary>
    /// 아이템 이미지 반환
    /// </summary>
    /// <returns></returns>
    public Sprite GetItemImage()
    {
        return _itemData.ItemIcon;
    }

    /// <summary>
    /// 각 아이템별 능력 사용
    /// </summary>
    public virtual void ItemUse()
    {
        
    }
    
}
