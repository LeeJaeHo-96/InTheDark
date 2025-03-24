using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item_FishingRod : Item
{
    private BoxCollider _fishingRodCollider;
    private Coroutine _attackCo;
    private Animator _animator;
    private Vector3 _originPos;
    private float _animationLength;
    private bool _isAttacked;
    
    protected override void Awake()
    {
        base.Awake();
        _fishingRodCollider = GetComponent<BoxCollider>(); 
    }

    public override void SetItemHoldPosition(Transform holdPos, float mouseX, float mouseY)
    {
        if (!_isAttacked)
        {
            transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
            
            if (holdPos.TryGetComponent(out PhotonView view))
            {
                photonView.RPC(nameof(SyncSetParentRPC), RpcTarget.AllViaServer, view.ViewID, false);
            }
        }
        else
        { 
            if (holdPos.TryGetComponent(out PhotonView view))
            {
                photonView.RPC(nameof(SyncSetParentRPC), RpcTarget.AllViaServer, view.ViewID, true);
            }
             
            photonView.RPC(nameof(SyncWeaponRotateRPC), RpcTarget.AllViaServer,holdPos.position.x, holdPos.position.y,holdPos.position.z, 90f);
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
    private void SyncWeaponRotateRPC(float posX, float posY, float posZ, float rotateY)
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
