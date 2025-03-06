using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviourPun
{
    //TODO: 전체적인 수치는 조정 필요
    private float _walkSpeed = 5f;

    public float WalkSpeed
    {
        get => _walkSpeed;
    }

    private float _runSpeed = 10f;

    public float RunSpeed
    {
        get => _runSpeed;
    }

    private float _jumpPower = 3f;

    public float JumpPower
    {
        get => _jumpPower;
    }

    private float _stamina = 100f;

    public float Stamina
    {
        get => _stamina;
    }

    public UnityAction<float> OnChangedStamina;
    
    /// <summary>
    /// 아이템을 들었을 때 이동 속도 감소
    /// </summary>
    /// <param name="value"></param>
    public void IsHoldingItem(float value)
    {
        //TODO: 아이템을 잡았을 때 얼마나 이동 속도를 줄일지 생각
        float speedFactor = value / 10f;
        
        _walkSpeed -= speedFactor;
        _runSpeed -= speedFactor;
    }

    /// <summary>
    /// 아이템을 버렸을 때 이동 속도 복구
    /// </summary>
    /// <param name="value"></param>
    public void IsNotHoldingItem(float value)
    {
        float speedRecovery = value / 10f;
        _walkSpeed += speedRecovery;
        _runSpeed += speedRecovery;
    }

    /// <summary>
    /// 스태미너 감소
    /// </summary>
    /// <param name="value"></param>
    public void ConsumeStamina(float value)
    {
        _stamina -= value;
        _stamina = Mathf.Clamp(_stamina, 0, _stamina);
        OnChangedStamina?.Invoke(_stamina);
    }

    /// <summary>
    /// 스태미너 회복
    /// </summary>
    /// <param name="value"></param>
    public void RecoverStamina(float value)
    {
        _stamina += value;
        _stamina = Mathf.Clamp(_stamina, 0, _stamina); 
        OnChangedStamina?.Invoke(_stamina);
    }
}
