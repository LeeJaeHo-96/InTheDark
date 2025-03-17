using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerRun : PlayerState
{
    private Coroutine _consumeCo;
    
    public PlayerRun(PlayerController controller) : base(controller) {}

    public override void Enter()
    { 
        _controller.PlayerAnimator.SetBool(_runAniHash, true);
        
        if (_consumeCo == null)
        {
            _consumeCo = _controller.StartCoroutine(UseStaminaRoutine());
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
        else if (_controller.MoveDir.z <= 0 || !Input.GetKey(KeyCode.LeftShift) || _controller.PlayerStats.Stamina <= 0f)
        {
            _controller.ChangeState(PState.WALK);
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
        
        _controller.PlayerRb.MovePosition(_controller.PlayerRb.position + dir.normalized * _controller.PlayerStats.RunSpeed * Time.fixedDeltaTime);
        
    }

    public override void Exit()
    {
        _controller.PlayerAnimator.SetBool(_runAniHash, false);
        _controller.StopCoroutine(_consumeCo);
        _consumeCo = null;
    }
    

    
}
