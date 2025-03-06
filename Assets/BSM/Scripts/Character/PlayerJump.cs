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
    
    //TODO: 이동 중 점프했을 때 처리를 해주는게 자연스럽나?
    // public override void FixedUpdate()
    // {
    //     Vector3 dir = _controller.transform.TransformDirection(_controller.MoveDir);
    //     
    //     _controller.PlayerRb.MovePosition(_controller.transform.position + dir.normalized * _controller.PlayerStats.WalkSpeed * Time.fixedDeltaTime);   
    // }
}
