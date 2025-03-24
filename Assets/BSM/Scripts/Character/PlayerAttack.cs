using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{
    public PlayerAttack(PlayerController controller) : base(controller){}

    public override void Enter()
    {
        if (_controller.CurCarryItem == null)
        {
            _controller.ChangeState(PState.IDLE);
        }
        else
        { 
            if (_controller.CurCarryItem.AttackItem())
            {
                if (_controller.CurCarryItem.GetHoldingType() == ItemHoldingType.ONEHANDED)
                {
                    _controller.BehaviourAnimation(_attackAniHash);
                }
                
                _controller.CurCarryItem.ItemUse(_controller.PlayerAnimator, _controller);
            }
            else
            {
                _controller.CurCarryItem.ItemUse();
            } 
        }
    }

    public override void OnTrigger()
    {
        _controller.ChangeState(PState.HURT);
    }
    
    public override void Update()
    {
        if (_controller.MoveDir == Vector3.zero)
        {
            _controller.ChangeState(PState.IDLE);
        }
        else if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) || _controller.MoveDir.z < 0 && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        else if (_controller.MoveDir.z > 0  && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
    }
 
    
}
