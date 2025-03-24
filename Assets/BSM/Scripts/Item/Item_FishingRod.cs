using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item_FishingRod : Item
{
    private BoxCollider _fishingRodCollider;
    private Coroutine _attackCo;
    private Transform _holdPos;
    
    private Vector3 _originPos;
    private float _animationLength;
    private float _animationSpeed;
    private bool _isAttacked;
    
    protected override void Awake()
    {
        base.Awake();
        _fishingRodCollider = GetComponent<BoxCollider>(); 
    }

    public override void SetItemHoldPosition(Transform holdPos, float mouseX, float mouseY)
    {
        _holdPos = holdPos;
        transform.position = holdPos.position;
        if (!_isAttacked)
        {
            transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
            transform.SetParent(null);
        }
        else
        { 
            photonView.RPC(nameof(SyncParentRPC), RpcTarget.AllViaServer);
            transform.localRotation = Quaternion.Euler(0, 90,0);
        }
    }

    [PunRPC]
    private void SyncParentRPC()
    {
        transform.SetParent(_holdPos);
    }
    
    public override void ItemUse(Animator animator)
    {
        animator.SetFloat(_attackSpeedAniHash, _itemData.AttackSpeed);
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
