using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviourPun
{
    public bool CanCarry;
  
    //TODO: 전체적인 수치는 조정 필요
    private float _walkSpeed = 2f;

    private int _curHP;

    public int CurHP
    {
        get => _curHP;
        set
        {
            _curHP = value; 
        }
    }
    
    private int _maxHP = 100;
    
    public float WalkSpeed
    {
        get => _walkSpeed;
    }

    private float _runSpeed = 5f;

    public float RunSpeed
    {
        get => _runSpeed;
    }

    private float _jumpPower = 5f;

    public float JumpPower
    {
        get => _jumpPower;
    }

    public float MaxStamina => 100f;
    
    private float _stamina = 100f;

    public float Stamina
    {
        get => _stamina;
        set
        {
            _stamina = value;
        }
    }

    public UnityAction<float> OnChangedStamina;
    public UnityAction<int> OnChangedHealth;
    
    private void Awake()
    {
        CanCarry = true;

        //TODO: 임시HP 추후 DB에 저장되어 있는 정보가 있는지 확인 후 현재 hp를 반영하면 될듯
        _curHP = _maxHP; 
    }

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

        _walkSpeed = Mathf.Clamp(_walkSpeed, 1f, _walkSpeed);
        _runSpeed = Mathf.Clamp(_runSpeed, 1f, _runSpeed);

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
  
    [PunRPC]
    public void SyncHealthRPC(int hp)
    {
        _curHP = hp;
    }
    
}
