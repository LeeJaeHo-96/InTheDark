using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    
    public PlayerJump(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        _controller.PlayerRb.AddForce(Vector3.up * _controller.PlayerStats.JumpPower ,ForceMode.Impulse);
    }

    public override void Update()
    {
        //TODO: 추후 0이 아닌, 아래 방향으로 레이를 쏴서 그라운드와 접촉했는지 판단하고 변경하면 될듯
        if (_controller.PlayerRb.position.y <= 0f)
        {
            _controller.ChangeState(PState.IDLE);
        }
        
    }
    
}
