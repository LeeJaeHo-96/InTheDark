using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateMachine
{
    
    protected PlayerController _controller;
    
    public PlayerState(PlayerController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        //TODO: 각 초기 상태 초기화 작업
    }

    /// <summary>
    /// 스태미너 회복 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator RecoverStaminaRoutine()
    {
        while (_controller.PlayerStats.Stamina < 100f)
        {
            _controller.PlayerStats.RecoverStamina(5f);
            yield return new WaitForSeconds(0.8f);
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
            _controller.PlayerStats.ConsumeStamina(5f);
            yield return new WaitForSeconds(0.8f);
        }
    }
}
