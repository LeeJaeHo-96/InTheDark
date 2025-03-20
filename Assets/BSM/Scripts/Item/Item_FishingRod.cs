using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item_FishingRod : Item
{
    private BoxCollider _fishingRodCollider;
    private Coroutine _attackCo;

    private Vector3 _originPos;
    private bool _isReady;

    public float a;
    public float b;
    public float c;
    
    protected override void Awake()
    {
        base.Awake();
        _fishingRodCollider = GetComponent<BoxCollider>(); 
    }

    public override void SetItemHoldPosition(Transform holdPos, float mouseX, float mouseY)
    {  
        transform.position = holdPos.position; 
        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }
    
    public override void ItemUse(Animator animator)
    {
        animator.SetFloat(_attackSpeedAniHash, _itemData.AttackSpeed);
        _attackCo = StartCoroutine(AttackRoutine()); 
    }
 
    private IEnumerator AttackRoutine()
    {
        float elapsedTime = 0;
        float duration = _itemData.AttackSpeed; 

        if (Input.GetMouseButtonUp(0))
        {
            _isReady = true; 
            photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllBuffered, true);
        } 
        
        yield return new WaitForSeconds(0.5f);
        photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllBuffered, false);
         
        _isReady = false; 
    }
     
    protected override void ItemDrop()
    {
        if (_attackCo != null)
        {
            _fishingRodCollider.center = new Vector3(0, 0, 0.1f); 
            photonView.RPC(nameof(SyncAttackingRPC), RpcTarget.AllBuffered, false);
            _isReady = false;
            StopCoroutine(_attackCo);
            _attackCo = null;
        } 
    }
}
