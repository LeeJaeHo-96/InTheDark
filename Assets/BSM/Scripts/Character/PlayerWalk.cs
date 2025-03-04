using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    
    public PlayerWalk(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
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
        Vector3 dir = _controller.transform.TransformDirection(_controller.MoveDir);
        
        _controller.PlayerRb.MovePosition(_controller.transform.position + dir.normalized * _controller.PlayerStats.WalkSpeed * Time.fixedDeltaTime);   
    }
    
}
