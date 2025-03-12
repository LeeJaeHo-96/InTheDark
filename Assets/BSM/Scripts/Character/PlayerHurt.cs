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

            if (item.IsAttacking && item.AttackItem())
            {
                 _controller.PlayerStats.TakeDamage(item.GetItemDamage());

                 if (_controller.PlayerStats.CurHP <= 0)
                 {
                     _controller.ChangeState(PState.DEATH);
                 }
                 else
                 {
                     _controller.ChangeState(PState.IDLE);
                 }
            } 
        }
    } 
}
