using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachine
{
    
    protected PlayerController _controller;
    
    public PlayerState(PlayerController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        //TODO: 각 초기 상태 초기화 작업
    }

    
}
