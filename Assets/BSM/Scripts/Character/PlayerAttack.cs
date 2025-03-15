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
            _controller.CurCarryItem.ItemUse();
        }
    }

    public override void OnTrigger()
    {
        _controller.ChangeState(PState.HURT);
    }
    
    public override void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _controller.ChangeState(PState.IDLE);
        }
        else if (Input.GetMouseButton(0) && _controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        else if (Input.GetMouseButton(0) && _controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
    }
    
    
}
