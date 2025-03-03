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
        if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
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
    
    public override void Exit()
    {
        //Idle 애니메이션 종료
    }
}
