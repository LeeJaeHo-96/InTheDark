using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item_FishingRod : Item
{
    private Coroutine _attackCo;
    private Animator _animator;
    private Vector3 _originPos;
    private float _animationLength;
    private bool _isAttacked;
 
    public override void SetItemHoldPosition(Transform holdPos, float mouseX, float mouseY)
    {
        if (!_isAttacked)
        {
            transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
            ItemSetParent(holdPos, false); 
        }
        else
        {  
            ItemSetParent(holdPos, true);  
            photonView.RPC(nameof(SyncWeaponRotateRPC), RpcTarget.AllViaServer, 180f);
        }
    }
    
    /// <summary>
    /// 아이템 부모 동기화
    /// </summary>
    /// <param name="holdPos">아이템이 이동할 위치</param>
    /// <param name="isActive">활성화 여부</param>
    private void ItemSetParent(Transform holdPos, bool isActive)
    {
        if (holdPos.TryGetComponent(out PhotonView view))
        {
            photonView.RPC(nameof(SyncSetParentRPC), RpcTarget.AllViaServer, view.ViewID, isActive);
        }
    }
 
    [PunRPC]
    private void SyncSetParentRPC(int viewID, bool isCondition)
    {
        PhotonView view = PhotonView.Find(viewID);

        if (view != null)
        {
            transform.position = view.transform.position;
            transform.parent = isCondition ? view.transform : null; 
        } 
    }
    
    [PunRPC]
    private void SyncWeaponRotateRPC(float rotateY)
    { 
        transform.localRotation = Quaternion.Euler(0, rotateY, 0);
    }
    
    public override void ItemUse(Animator animator, PlayerController playerController)
    {
        playerController.BehaviourAnimation(_attackSpeedAniHash, _itemData.AttackSpeed);
        _animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        _isAttacked = true;
        _attackCo = StartCoroutine(AttackRoutine()); 
    }
 
    private IEnumerator AttackRoutine()
    {
        float elapsedTime = 0;
        
        photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllBuffered, true);

        //애니메이션 재생이 끝나면 공격 끝난 상태로 변경
        while (elapsedTime < _animationLength)
        { 
            elapsedTime += Time.deltaTime * _itemData.AttackSpeed; 
            yield return null;
        }
        
        _isAttacked = false;
        photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllBuffered, false);

    }
     
    protected override void ItemDrop()
    {
        if (_attackCo != null)
        { 
            photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllBuffered, false);
            StopCoroutine(_attackCo);
            _attackCo = null;
        } 
    }
}
