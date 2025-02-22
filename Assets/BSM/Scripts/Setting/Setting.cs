using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class Setting : ObjBind
{
    private GameManager _gameManager;

    private TextMeshProUGUI _cancelButtonText;
    private TMP_Dropdown _frameDropdown;
    private TMP_Dropdown _windowDropdown;
    private Slider _gammaSlider;
    private Button _confirmButton;
    private GameObject _changeNoticeText;
    
    //TODO: 다른 설정들 구현 완료하면 SIZE 변경 필요함
    private bool[] _changeCheck = new bool[3];
    private int _frameDropdownValue;

    private int _changeDetect => _changeCheck.Count(x => x);
    private Coroutine _changeDetectCo;
    
    private void Awake()
    {
        _gameManager = GameManager.Instance;
         
        Bind();
        Init();   
        SetFrameDropDown();
        SetWindownDropdown();
        SetGammaSlider();
    }

    private void OnEnable()
    {
        _changeDetectCo = StartCoroutine(SettingChangeDetectRoutine());
    }
    
    private void OnDisable()
    {
        ChangeCancel(); 
    }
    
    private void Init()
    {
        _cancelButtonText = GetComponentBind<TextMeshProUGUI>("Cancel_Button_Text");
        _confirmButton = GetComponentBind<Button>("Confirm_Button");
        _confirmButton.onClick.AddListener(ChangeSettingSave);

        _changeNoticeText = GetGameObjectBind("Changes_Confirm_Title");  
    }
    
    /// <summary>
    /// 변경 사항 있을 경우 안내 문구 활성화 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator SettingChangeDetectRoutine()
    {
        while (true)
        {
           _changeNoticeText.SetActive(_changeDetect > 0);

           _cancelButtonText.text = _changeDetect > 0 ? "   > 취소하기" : "   > 뒤로가기";
           
           yield return null;
        } 
    }
    
    /// <summary>
    /// 감마, 밝기 값 설정
    /// </summary>
    private void SetGammaSlider()
    {
        _gammaSlider = GetComponentBind<Slider>("Gamma_Bright_Slider");
        _gammaSlider.onValueChanged.AddListener(x => ChangeGammaBrightness(x));
        _gammaSlider.value = PlayerPrefs.HasKey("GammaBrightness") ? PlayerPrefs.GetFloat("GammaBrightness") : 0f;
    }

    
    /// <summary>
    /// 감마, 밝기 값 변경
    /// </summary>
    /// <param name="value"></param>
    private void ChangeGammaBrightness(float value)
    {
        float gammaBrightness = value;
        gammaBrightness = Mathf.Clamp(value, -1f, 1f);
        
        _gameManager.SetGammaBrightness(gammaBrightness);
        _changeCheck[(int)Settings.GAMMA] = !(Mathf.Approximately(_gameManager.CurGammaBrightness, PlayerPrefs.GetFloat("GammaBrightness")));
    }
 
    
    /// <summary>
    /// 프레임 드롭다운 메뉴 설정
    /// </summary>
    private void SetFrameDropDown()
    {
        _frameDropdown = GetComponentBind<TMP_Dropdown>("FrameDropdown");
        _frameDropdown.onValueChanged.AddListener(x => ChangeFrame(x));

        _frameDropdown.value = PlayerPrefs.HasKey("FrameDropdownValue") ? 
            PlayerPrefs.GetInt("FrameDropdownValue") : 0;
    }
    
    /// <summary>
    /// 드롭다운 메뉴 변경 기능
    /// </summary>
    /// <param name="value"></param>
    private void ChangeFrame(int value)
    {
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
        
        _frameDropdownValue = value; 
        _changeCheck[(int)Settings.FRAME] = _frameDropdownValue != PlayerPrefs.GetInt("FrameDropdownValue");
    }
    
    
    /// <summary>
    /// 윈도우 모드 드롭다운 메뉴 설정
    /// </summary>
    private void SetWindownDropdown()
    {
        _windowDropdown = GetComponentBind<TMP_Dropdown>("WindowDropdown");
        _windowDropdown.onValueChanged.AddListener(x => ChangeWindowMode(x));

        _windowDropdown.value = PlayerPrefs.HasKey("WindowMode") ? PlayerPrefs.GetInt("WindowMode") : 0;
    }
    
    /// <summary>
    /// 윈도우 모드 설정 드롭다운 변경
    /// </summary>
    /// <param name="value"></param>
    private void ChangeWindowMode(int value)
    {
        _gameManager.SetWindowMode(value);
        _changeCheck[(int)Settings.WINDOWMODE] = _gameManager.CurWindowMode != PlayerPrefs.GetInt("WindowMode");
    }

    /// <summary>
    /// 변경 사항 내역 저장
    /// </summary>
    private void ChangeSettingSave()
    {
        PlayerPrefs.SetInt("FrameRate", _gameManager.CurFrame);
        PlayerPrefs.SetInt("FrameDropdownValue", _frameDropdownValue);
        PlayerPrefs.SetInt("Vsync", _gameManager.CurVSyncCount);
        PlayerPrefs.SetInt("WindowMode", _gameManager.CurWindowMode);
        PlayerPrefs.SetFloat("GammaBrightness", _gameManager.CurGammaBrightness);
    }
    
    /// <summary>
    /// 변경 내용 복구
    /// </summary>
    private void ChangeCancel()
    {
        if (_changeDetectCo != null)
        {
            StopCoroutine(_changeDetectCo);
            _changeDetectCo = null;
        }

        _frameDropdown.value = PlayerPrefs.GetInt("FrameDropdownValue");
        _gameManager.SetFrames(PlayerPrefs.GetInt("FrameRate"));
        
        _windowDropdown.value = PlayerPrefs.GetInt("WindowMode");
        _gameManager.SetWindowMode(PlayerPrefs.GetInt("WindowMode"));
        
        _gameManager.SetGammaBrightness(PlayerPrefs.GetFloat("GammaBrightness"));
        _gammaSlider.value = PlayerPrefs.GetFloat("GammaBrightness");
        
        _changeNoticeText.SetActive(false);
        _cancelButtonText.text = "   > 뒤로가기"; 
    }
    
    
}
