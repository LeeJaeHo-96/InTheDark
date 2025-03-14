using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : ObjBind
{
    
    private PlayerStats _playerStats;
    private Slider _staminaSlider;
    private Slider _healthSlider;
    
    private void Awake()
    {
        Bind(); 
        _playerStats = GetComponent<PlayerStats>();
        _staminaSlider = GetComponentBind<Slider>("StaminaSlider");
        _healthSlider = GetComponentBind<Slider>("HealthSlider");
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
        _staminaSlider.value = value;
    }

    /// <summary>
    /// 체력바 UI 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void UpdateHealth(int value)
    {
        _healthSlider.value = value;
    }
    
    
}
