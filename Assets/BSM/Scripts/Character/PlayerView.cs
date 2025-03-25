using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : ObjBind
{
    
    private PlayerStats _playerStats;
    private Image _staminaValue;
    private Image _healthValue;
    
    
    private void Awake()
    {
        Bind(); 
        _playerStats = GetComponent<PlayerStats>();
        _staminaValue = GetComponentBind<Image>("StaminaValue");
        _healthValue = GetComponentBind<Image>("HealthValue");
        
    }

    private void Start()
    {
        UpdateStamina(_playerStats.Stamina);
        UpdateHealth(_playerStats.CurHP);
    }

    private void OnEnable()
    {
        _playerStats.OnChangedStamina += UpdateStamina;
        _playerStats.OnChangedHealth += UpdateHealth;
    }

    private void OnDisable()
    {
        _playerStats.OnChangedStamina -= UpdateStamina;
        _playerStats.OnChangedHealth -= UpdateHealth;
    }

    /// <summary>
    /// 스태미너 UI 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void UpdateStamina(float value)
    {
        _staminaValue.fillAmount = value * 0.01f;
    }

    /// <summary>
    /// 체력바 UI 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void UpdateHealth(int value)
    {
        _healthValue.fillAmount = value * 0.01f;
    }
    
    
}
