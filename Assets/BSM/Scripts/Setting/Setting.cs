using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class Setting : ObjBind
{
    private TMP_Dropdown _frameDropdown;
    private TMP_Dropdown _windowDropdown;

    private Slider _gammaSlider;
    
    private GameManager _gameManager;
    
    private void Awake()
    {
        Bind();
        _gameManager = GameManager.Instance;

        SetFrameDropDown();
        SetWindownDropdown();
        SetGammaSlider();
    }

    /// <summary>
    /// 프레임 드롭다운 메뉴 설정
    /// </summary>
    private void SetFrameDropDown()
    {
        _frameDropdown = GetComponentBind<TMP_Dropdown>("FrameDropdown");
        _frameDropdown.onValueChanged.AddListener(x => ChangeFrame(x));
         
        if (PlayerPrefs.HasKey("FrameDropDownValue"))
        {
            _frameDropdown.value = PlayerPrefs.GetInt("FrameDropDownValue");
        }
        else
        {
            _frameDropdown.value = 0;
        }
    }

    /// <summary>
    /// 윈도우 모드 드롭다운 메뉴 설정
    /// </summary>
    private void SetWindownDropdown()
    {
        _windowDropdown = GetComponentBind<TMP_Dropdown>("WindowDropdown");
        _windowDropdown.onValueChanged.AddListener(x => ChangeWindowMode(x));

        if (PlayerPrefs.HasKey("WindowDropdownValue"))
        {
            _windowDropdown.value = PlayerPrefs.GetInt("WindowDropdownValue");
        }
        else
        {
            _windowDropdown.value = 0;
        }
        
    }
    
    /// <summary>
    /// 감마, 밝기 변경
    /// </summary>
    private void SetGammaSlider()
    {
        _gammaSlider = GetComponentBind<Slider>("Gamma_Bright_Slider");

        _gammaSlider.onValueChanged.AddListener(x =>
        {
            float gammaBrightness = x;
            gammaBrightness = Mathf.Clamp(x, -1f, 1f);
            PlayerPrefs.SetFloat("GammaBrightnessValue", gammaBrightness);
            
            _gameManager.SetGammaBrightness(gammaBrightness);
        });
        
        if (PlayerPrefs.HasKey("GammaBrightnessValue"))
        {
            _gammaSlider.value = PlayerPrefs.GetFloat("GammaBrightnessValue");
        }
        else
        {
            _gammaSlider.value = 0f;
        } 
    }
    
    
    /// <summary>
    /// 드롭다운 메뉴 변경 기능
    /// </summary>
    /// <param name="value"></param>
    private void ChangeFrame(int value)
    {
        PlayerPrefs.SetInt("FrameDropDownValue", value);
        
        switch (value)
        {
            case 0 :
                _gameManager.SetFrames(Screen.currentResolution.refreshRate);
                break;
            
            case 1 :
                _gameManager.SetFrames(-1, true);
                break;
            
            case 2 :
                _gameManager.SetFrames(144);
                break;
            case 3 :
                _gameManager.SetFrames(120);
                break;
            
            case 4 :
                _gameManager.SetFrames(60); 
                break;
            
            case 5 :
                _gameManager.SetFrames(30);
                break;
        }
    }
    
    /// <summary>
    /// 윈도우 모드 설정 드롭다운 변경
    /// </summary>
    /// <param name="value"></param>
    private void ChangeWindowMode(int value)
    {
        PlayerPrefs.SetInt("WindowDropdownValue", value); 
        _gameManager.SetWindowMode(value);  
        
    }
    
    
}
