using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    private bool canJump;
    
    public PlayerJump(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        if (_controller.PlayerRb.velocity == Vector3.zero)
        {
            _controller.PlayerRb.AddForce(Vector3.up * _controller.PlayerStats.JumpPower ,ForceMode.Impulse);
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
}
