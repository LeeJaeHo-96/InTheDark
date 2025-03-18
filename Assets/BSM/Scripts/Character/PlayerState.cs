using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachine
{
    protected static Coroutine _staminaRecoverCo; 
    protected PlayerController _controller;

    protected int _walkAniHash;
    protected int _runAniHash;
    protected int _dirXAniHash;
    protected int _dirZAniHash;
    protected int _idleAniHash;
    protected int _jumpAniHash;
    protected int _hitAniHash;
    protected int _deathAniHash; 
    
    public PlayerState(PlayerController controller)
    {
        _controller = controller;
        
        _walkAniHash = Animator.StringToHash("IsWalk");
        _runAniHash = Animator.StringToHash("IsRun");
        _dirXAniHash = Animator.StringToHash("DirX");
        _dirZAniHash = Animator.StringToHash("DirZ");
        _idleAniHash = Animator.StringToHash("IsIdle");
        _jumpAniHash = Animator.StringToHash("IsJump");
        _hitAniHash = Animator.StringToHash("IsHit");
        _deathAniHash = Animator.StringToHash("IsDeath");
    }
 
    /// <summary>
    /// 스태미너 회복
    /// </summary>
    protected void RecoverStamina()
    {
        if (_controller.PlayerStats.Stamina < 100f && _staminaRecoverCo == null)
        {
            _staminaRecoverCo = _controller.StartCoroutine(RecoverStaminaRoutine());
        }
    }
    
    /// <summary>
    /// 스태미너 회복 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverStaminaRoutine()
    {
        while (_controller.PlayerStats.Stamina <= 100f)
        {
            yield return new WaitForSeconds(0.8f); 
            _controller.PlayerStats.Stamina += 5;
            _controller.PlayerStats.Stamina = Mathf.Clamp(_controller.PlayerStats.Stamina, 0, _controller.PlayerStats.MaxStamina);
            _controller.PlayerStats.OnChangedStamina?.Invoke(_controller.PlayerStats.Stamina); 
        }  
    }
    
    /// <summary>
    /// 스태미너 사용 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator UseStaminaRoutine()
    {
        while (_controller.CurState == PState.RUN)
        {
            _controller.PlayerStats.Stamina -= 5;
            _controller.PlayerStats.Stamina = Mathf.Clamp(_controller.PlayerStats.Stamina, 0, _controller.PlayerStats.Stamina);
            _controller.PlayerStats.OnChangedStamina?.Invoke(_controller.PlayerStats.Stamina);
            yield return new WaitForSeconds(0.8f);
        } 
    }
    
    /// <summary>
    /// 체력 회복 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator RecoverHealthRoutine()
    {  
        //TODO: 체력 회복 임시 조건
        while (_controller.PlayerStats.CurHP <= 40f)
        { 
            _controller.PlayerStats.CurHP += 1;
            _controller.PlayerStats.OnChangedHealth?.Invoke(_controller.PlayerStats.CurHP);
            yield return new WaitForSeconds(1f);
        }  
    }
    
}
