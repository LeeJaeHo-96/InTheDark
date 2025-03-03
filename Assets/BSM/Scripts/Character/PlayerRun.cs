using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerState
{
    
    public PlayerRun(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        Debug.Log("플레이어 뛰는 상태 진입");
    }

    public override void Update()
    {
        Debug.Log("뛰는 중");
        
        if (_controller.MoveDir == Vector3.zero)
        {
            _controller.ChangeState(PState.IDLE);
        }
        else if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        
    }

    public override void FixedUpdate()
    {
        _controller.PlayerRb.MovePosition(_controller.PlayerRb.position + _controller.MoveDir * _controller.PlayerStats.RunSpeed * Time.fixedDeltaTime);
        
    }
    
}
