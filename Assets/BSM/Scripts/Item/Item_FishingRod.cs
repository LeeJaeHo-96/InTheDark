using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item_FishingRod : Item
{
    private CapsuleCollider _fishingRodCollider;
    private Coroutine _attackCo;

    private Vector3 _originPos;
    private bool _isReady;
    
    protected override void Awake()
    {
        base.Awake();
        _fishingRodCollider = GetComponent<CapsuleCollider>(); 
    }
     
    public override void ItemUse()
    {
        _attackCo = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        float elapsedTime = 0;
        float duration = _itemData.AttackSpeed;

        //무기의 사정거리만큼 콜라이더 범위 변경
        _fishingRodCollider.center = new Vector3(0, 0, _attackRange);

        while (!_isReady)
        {
            if (Input.GetMouseButton(0))
            {
                //TODO: 임시 모션
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90f, 0);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _isReady = true; 
                photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllViaServer, true);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllViaServer, false);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _isReady = false;
        
        //콜라이더 범위 복구
        _fishingRodCollider.center = new Vector3(0, 0, 0.18f); 
    }
     
    protected override void ItemDrop()
    {
        if (_attackCo != null)
        {
            StopCoroutine(_attackCo);
            _attackCo = null;
        } 
    }
}
