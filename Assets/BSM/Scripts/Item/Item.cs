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
    
    private Rigidbody _itemRb;
    protected Collider _itemCollider;
    
    protected InteractableItemData _itemData; 
 
    public bool IsOwned;
    public bool IsAttacking;
    
    protected float _attackRange;
    protected float _damage;
    public float _battery;
    protected bool isPower = false;
    
    private float _itemWeight;
    

    
    protected virtual void Awake()
    {
        _itemRb = GetComponent<Rigidbody>();
        _itemCollider = GetComponent<Collider>();
    }

    protected void Start()
    {
        StartCoroutine(ItemSetDelayRoutine());
    }
  
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tag.Ground))
        {
            _itemCollider.isTrigger = true;
            _itemRb.isKinematic = true; 
        }
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
    }
    
    /// <summary>
    /// 아이템 데이터 자동 할당
    /// </summary>
    private void SetItemData()
    {
        _itemData = ItemManager.Instance.GetItemData(_itemID);
        _itemWeight = _itemData.ItemWeight;
        _attackRange = _itemData.AttackRange;
        _damage = _itemData.Damage;
        _battery = _itemData.MaxDurability; 
    }
 
    /// <summary>
    /// 아이템 주운 후 동작
    /// </summary>
    /// <param name="player"></param>
    public void PickUp(Player player)
    {
        photonView.TransferOwnership(player);
        photonView.RPC(nameof(SyncOwnershipRPC), RpcTarget.AllViaServer, true);
    }

    /// <summary>
    /// 아이템 버린 후 동작
    /// </summary>
    /// <param name="player"></param>
    public void Drop(Player player = null)
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        _itemRb.isKinematic = false;
        ItemDrop();
        photonView.TransferOwnership(null);
        photonView.RPC(nameof(SyncItemTriggerRPC), RpcTarget.AllViaServer, false);
        photonView.RPC(nameof(SyncOwnershipRPC), RpcTarget.AllViaServer, false); 
    } 
    
    [PunRPC]
    protected void SyncItemTriggerRPC(bool isTrigger)
    {
        _itemCollider.isTrigger = isTrigger;
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
    public float GetItemWeight() => _itemWeight; 

    /// <summary>
    /// 아이템 타입
    /// </summary>
    /// <returns></returns>
    public ItemHoldingType GetHoldingType() => _itemData.ItemHoldingType;
 
    /// <summary>
    /// 아이템 이미지 반환
    /// </summary>
    /// <returns></returns>
    public Sprite GetItemImage() => _itemData.ItemIcon;

    /// <summary>
    /// 아이템 공격 타입 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool AttackItem() => _itemData.IsAttack;

    public int GetItemDamage() => _itemData.Damage;
    
    /// <summary>
    /// 각 아이템별 능력 사용
    /// </summary>
    public virtual void ItemUse() {}

     
    /// <summary>
    /// 아이템 별 초기화 작업
    /// </summary>
    protected virtual void ItemRestore() {}
    
    /// <summary>
    /// 아이템 버린 후 동작
    /// </summary>
    protected virtual void ItemDrop() {}

    [PunRPC]
    protected void SyncAttackingRPC(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }
    
}
