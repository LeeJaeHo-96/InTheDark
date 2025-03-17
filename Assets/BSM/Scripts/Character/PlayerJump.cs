using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    public PlayerJump(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        _controller.PlayerAnimator.SetBool(_jumpAniHash, true);
        _controller.PlayerRb.AddForce(Vector3.up * _controller.PlayerStats.JumpPower ,ForceMode.Impulse);
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
        else if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _controller.ChangeState(PState.ATTACK);
        }
    }

    public override void Exit()
    {
        _controller.PlayerAnimator.SetBool(_jumpAniHash, false);
    }

}
