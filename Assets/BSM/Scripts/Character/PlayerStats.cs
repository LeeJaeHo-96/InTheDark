using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviourPun
{
    public bool CanCarry;

    public int TempHP;

    private void Update()
    {
        TempHP = _curHP;
    }

    //TODO: 전체적인 수치는 조정 필요
    private float _walkSpeed = 5f;

    private int _curHP;

    public int CurHP
    {
        get => _curHP;
    }
    
    private int _maxHP = 100;
    
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
    
    /// <summary>
    /// 현재 체력 감소
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        _curHP -= damage;
        OnChangedHealth?.Invoke(_curHP);
        photonView.RPC(nameof(SyncHealthRPC), RpcTarget.AllViaServer, _curHP); 
    }

    [PunRPC]
    private void SyncHealthRPC(int hp)
    {
        _curHP = hp;
    }
    
}
