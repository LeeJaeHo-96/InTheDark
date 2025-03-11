using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FishingRod : Item
{
    private CapsuleCollider _fishingRodCollider;
    private Coroutine _attackCo;

    private bool _isReady;
    
    protected override void Awake()
    {
        base.Awake();
        _fishingRodCollider = GetComponent<CapsuleCollider>(); 
    }
    
    
    
    public override void ItemUse()
    {
        Debug.Log("휘두를 준비"); 
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
            Debug.Log("공격 대기중");
            
            if (Input.GetMouseButtonUp(0))
            {
                _isReady = true;
            }

            yield return null;
        }
        
        Debug.Log("공격 완료 + 데미지 줌");
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("공격 애니메이션 완료 처리");
            yield return null;
        }
        
        //휘두름
        
        
        _isReady = false;
        
        //콜라이더 범위 복구
        _fishingRodCollider.center = new Vector3(0, 0, 0.18f); 
    }
     
    protected override void ItemDrop()
    {
        Debug.Log("삽 버림");
    }
}
