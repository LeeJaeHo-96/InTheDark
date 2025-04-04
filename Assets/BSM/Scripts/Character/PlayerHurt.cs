using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine; 
using System;

public class PlayerHurt : PlayerState
{
    public PlayerHurt(PlayerController controller) : base(controller)
    {
    }


    public override void Enter()
    { 
        ValidateHit(); 
    }

    public override void Update()
    {
        if (_controller.PlayerStats.CurHP <= 0)
        {
            _controller.ChangeState(PState.DEATH);
        }
        else
        {
            _controller.ChangeState(PState.IDLE);
        }
        
    }
  
    private void ValidateHit()
    {
        if (_controller.OnTriggerOther.gameObject.layer == GameManager.Instance.ItemLayerIndexValue)
        {
            Item item = _controller.OnTriggerOther.gameObject.GetComponent<Item>();
            
            //Null Reference방지
            if (item.photonView.Owner == null)
            {
                _controller.ChangeState(PState.IDLE);
                return;
            }
            
            //내가 휘두른 무기에 맞는 행위 방지
            if (item.photonView.Owner.Equals(_controller.photonView.Owner))
            {
                _controller.ChangeState(PState.IDLE);
                return;
            }
            
            if (item.IsAttacking && item.AttackItem())
            { 
                _controller.BehaviourAnimation(_hitAniHash); 
                TakeDamage(item.GetItemDamage());  
            } 
        }
        else if (_controller.OnTriggerOther.gameObject.layer == GameManager.Instance.MonsterLayerIndexValue)
        {
            Monster monster = _controller.OnTriggerOther.gameObject.GetComponent<Monster>();
            Debug.Log($"monster에는 이거가 {monster.GetComponent<PhotonView>().ViewID}");
            monster.isAttacked = true;
            
            if (monster.isAttacked)
            {
                monster.isAttacked = false;
                Debug.Log("플레이어가 맞음");
                TakeDamage(monster.pirateDamage);
            } 
        }
    }

    /// <summary>
    /// 현재 체력 감소
    /// </summary>
    /// <param name="damage"></param>
    private void TakeDamage(int damage)
    {
        _controller.PlayerStats.CurHP -= damage;
        _controller.photonView.RPC(nameof(_controller.PlayerStats.SyncHealthRPC), RpcTarget.AllBuffered,
            _controller.PlayerStats.CurHP);
        _controller.PlayerStats.OnChangedHealth?.Invoke(_controller.PlayerStats.CurHP);

    }
}