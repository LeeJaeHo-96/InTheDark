using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerIdle : PlayerState
{

    public PlayerIdle(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        Debug.Log("Idle Enter 진입");
    }

    public override void Update()
    {
        Debug.Log("대기 중");
        if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
    }
    
    public override void Exit()
    {
        //Idle 애니메이션 종료
    }
}
