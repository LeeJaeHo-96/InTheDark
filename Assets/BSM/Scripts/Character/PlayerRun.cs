using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerState
{
    
    public PlayerRun(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
    }

    public override void FixedUpdate()
    {
        Vector3 dir = _controller.transform.TransformDirection(_controller.MoveDir);
        
        _controller.PlayerRb.MovePosition(_controller.PlayerRb.position + dir.normalized * _controller.PlayerStats.RunSpeed * Time.fixedDeltaTime);
        
    }
    
}
