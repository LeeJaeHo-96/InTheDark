using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : PlayerState
{ 
    public PlayerHurt(PlayerController controller) : base(controller){}
     
    public override void Enter()
    {
        if (_controller.OnTriggerOther.gameObject.layer == _controller.ItemLayerIndexValue)
        {
            Item item = _controller.OnTriggerOther.gameObject.GetComponent<Item>();
            Debug.Log("무기에 닿앗음");
            
            if (item.IsAttacking && item.AttackItem())
            {
                 _controller.PlayerStats.TakeDamage(item.GetItemDamage());
                Debug.Log("데미지 받았음");
                 if (_controller.PlayerStats.CurHP <= 0)
                 {
                     _controller.ChangeState(PState.DEATH);
                 }
                 else
                 {
                     Debug.Log("아이들 상태 전환");
                     _controller.ChangeState(PState.IDLE);
                 }
            }
            
            _controller.ChangeState(PState.IDLE);
        }
    } 
}
