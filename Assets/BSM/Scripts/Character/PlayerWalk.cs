using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    
    public PlayerWalk(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        Debug.Log("플레이어 걷는 상태 진입");
    }

    public override void Update()
    {
        if (_controller.MoveDir == Vector3.zero)
        {
            _controller.ChangeState(PState.IDLE);
        }
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
    }
    
    public override void FixedUpdate()
    {
        _controller.PlayerRb.MovePosition(_controller.PlayerRb.position + _controller.MoveDir * _controller.PlayerStats.WalkSpeed * Time.fixedDeltaTime);
        
    }
    
}
