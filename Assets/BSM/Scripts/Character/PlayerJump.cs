using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    
    public PlayerJump(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        Debug.Log("점프 상태 진입");
        _controller.PlayerRb.AddForce(Vector3.up * _controller.PlayerStats.JumpPower ,ForceMode.Impulse);
    }

    public override void Update()
    {
        if (_controller.PlayerRb.position.y <= 0f)
        {
            _controller.ChangeState(PState.IDLE);
        }
        
    }
    
}
