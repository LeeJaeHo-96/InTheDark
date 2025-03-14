using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerHurt : PlayerState
{
    public PlayerHurt(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
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
                TakeDamage(item.GetItemDamage());  
            } 
        }
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