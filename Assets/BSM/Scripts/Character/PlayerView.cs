using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : ObjBind
{
    
    private PlayerStats _playerStats;
    private Slider _staminaSlider;

    private void Awake()
    {
        Bind(); 
        _playerStats = GetComponent<PlayerStats>(); 
        _staminaSlider = GetComponentBind<Slider>("StaminaSlider");
    }

    private void Start()
    {
        UpdateStamina(_playerStats.Stamina);
    }

    private void OnEnable()
    {
        _playerStats.OnChangedStamina += UpdateStamina;
    }

    private void OnDisable()
    {
        _playerStats.OnChangedStamina -= UpdateStamina;
    }

    /// <summary>
    /// 스태미너 UI 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void UpdateStamina(float value)
    {
        _staminaSlider.value = value;
    }

}
