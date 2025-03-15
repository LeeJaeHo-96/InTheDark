using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachine
{
    protected Coroutine _healthRecoverCo;
    protected Coroutine _staminaRecoverCo; 
    protected PlayerController _controller;
    protected static bool isRecovering; 
    
    public PlayerState(PlayerController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        //TODO: 각 초기 상태 초기화 작업
    }

    /// <summary>
    /// 스태미너 회복
    /// </summary>
    protected void RecoverStamina()
    {
        if (_controller.PlayerStats.Stamina < 100f && !isRecovering)
        {
            isRecovering = true;
            _staminaRecoverCo = _controller.StartCoroutine(RecoverStaminaRoutine());
        }
    }
    
    /// <summary>
    /// 스태미너 회복 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverStaminaRoutine()
    {
        while (_controller.PlayerStats.Stamina < 100f)
        {
            _controller.PlayerStats.Stamina += 5;
            _controller.PlayerStats.Stamina = Mathf.Clamp(_controller.PlayerStats.Stamina, 0, _controller.PlayerStats.MaxStamina);
            _controller.PlayerStats.OnChangedStamina?.Invoke(_controller.PlayerStats.Stamina);
            yield return new WaitForSeconds(0.8f); 
        }  
        isRecovering = false;
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
         
        while (_controller.PlayerStats.CurHP <= 20f)
        {
            yield return null;
        }
        
    }
    
}
