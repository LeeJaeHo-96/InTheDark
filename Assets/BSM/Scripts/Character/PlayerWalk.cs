using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    
    public PlayerWalk(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        RecoverStamina();
    }

    public override void OnTrigger()
    {
        _controller.ChangeState(PState.HURT);
    }
    
    public override void Update()
    {
        if (_staminaRecoverCo != null && !isRecovering && _controller.PlayerStats.Stamina >= 100f)
        {
            Debug.Log("코루틴 중지");
            _controller.StopCoroutine(_staminaRecoverCo);
            _staminaRecoverCo = null;
        }
        
        if (_controller.MoveDir == Vector3.zero)
        {
            _controller.ChangeState(PState.IDLE);
        }
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
        else if (Input.GetMouseButtonDown(0) && _controller.CurCarryItem != null)
        {
            _controller.CurCarryItem.ItemUse();
        }

    }
    
    public override void FixedUpdate()
    {
        Vector3 dir = _controller.transform.TransformDirection(_controller.MoveDir);
        
        _controller.PlayerRb.MovePosition(_controller.transform.position + dir.normalized * _controller.PlayerStats.WalkSpeed * Time.fixedDeltaTime);   
    }
 
}
