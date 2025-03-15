using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerController controller) : base(controller) {}

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
        else if (_controller.MoveDir == Vector3.zero && Input.GetMouseButtonDown(0))
        {
            _controller.ChangeState(PState.ATTACK);
        }
        
    }
     
    
    //TODO: 가만히 있는 상태이고 체력이 20이하이면 20까지 체력이 차야함
    public override void Exit()
    {
        if (!isRecovering && _staminaRecoverCo != null)
        {
            Debug.Log("코루틴 중지");
            _controller.StopCoroutine(_staminaRecoverCo);
            _staminaRecoverCo = null;
        }
    }
    
}
