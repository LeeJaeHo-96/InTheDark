using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    private Coroutine _recoverHealthCo;
    private Coroutine _recoverWaitCo;
    
    public PlayerIdle(PlayerController controller) : base(controller) {}

    public override void Enter()
    { 
        RecoverStamina(); 
        _controller.BehaviourAnimation(_idleAniHash, true);
        
        //TODO: 체력 회복 임시 조건
        if (_controller.PlayerStats.CurHP <= 20f)
        { 
            _recoverWaitCo = _controller.StartCoroutine(RecoverHealthWaitRoutine());
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
            _controller.StopCoroutine(_staminaRecoverCo);
            _staminaRecoverCo = null;
        }
        
        if (_controller.MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) || _controller.MoveDir.z < 0 && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.WALK);
        }
        else if (_controller.MoveDir.z > 0  && Input.GetKey(KeyCode.LeftShift))
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

    public override void Exit()
    { 
        _controller.BehaviourAnimation(_idleAniHash, false);
        
        if (_recoverWaitCo != null)
        {
            _controller.StopCoroutine(_recoverWaitCo);
            _recoverWaitCo = null;
        }

        if (_recoverHealthCo != null)
        {
            _controller.StopCoroutine(_recoverHealthCo);
            _recoverHealthCo = null;
        } 
    }
    
    /// <summary>
    /// 체력 회복 전 대기 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverHealthWaitRoutine()
    {
        //TODO: 체력 회복 임시 조건
        yield return new WaitForSeconds(3f);
        _recoverHealthCo = _controller.StartCoroutine(RecoverHealthRoutine()); 
    }
    
}
