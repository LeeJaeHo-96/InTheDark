using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    public PlayerJump(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        float remainStamina = _controller.PlayerStats.Stamina; 
        
        if (remainStamina - 5f > 0)
        {
            _controller.BehaviourAnimation(_jumpAniHash);
            _controller.PlayerRb.AddForce(Vector3.up * _controller.PlayerStats.JumpPower ,ForceMode.Impulse);
            _controller.PlayerStats.Stamina -= 5f;
            _controller.PlayerStats.OnChangedStamina?.Invoke(_controller.PlayerStats.Stamina);
        }
        
    }

    public override void OnTrigger()
    {
        _controller.ChangeState(PState.HURT);
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
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _controller.ChangeState(PState.ATTACK);
        }
    }
 
}
