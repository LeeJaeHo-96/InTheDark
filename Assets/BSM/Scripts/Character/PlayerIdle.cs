using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    private Coroutine _recoverHealthCo;
    
    public PlayerIdle(PlayerController controller) : base(controller) {}

    public override void Enter()
    { 
        RecoverStamina();

        if (_controller.PlayerStats.CurHP <= 20f)
        {
            _recoverHealthCo = _controller.StartCoroutine(RecoverHealthRoutine());
        }
        
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
        
        if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
        else if (_controller.MoveDir == Vector3.zero && Input.GetMouseButtonDown(0))
        {
            _controller.ChangeState(PState.ATTACK);
        } 
    }

    
}
