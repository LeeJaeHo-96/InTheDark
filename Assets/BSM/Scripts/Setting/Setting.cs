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
    private SoundManager _soundManager;
    private DataManager _dataManager;
    
    
    private TextMeshProUGUI _cancelButtonText;
    private TMP_Dropdown _frameDropdown;
    private TMP_Dropdown _windowDropdown;
    private Slider _gammaSlider;
    private Slider _masterVolumeSlider;
    private Slider _sfxVolumeSlider;
    private Slider _bgmVolumeSlider;
    private Slider _sensitivitySlider;
    private Button _confirmButton;
    private Button _resetButton;
    
    private GameObject _changeNoticeText;
    
    private Dictionary<int, int> _frameDict = new Dictionary<int, int>()
    {
        {0, _refreshRate},
        {1, -1},
        {2, 144},
        {3, 120},
        {4, 60},
        {5, 30}
    };

    private static int _refreshRate;
    
    //TODO: 다른 설정들 구현 완료하면 SIZE 변경 필요함
    private bool[] _changeCheck = new bool[(int)Settings.SIZE];
    private int _frameDropdownValue;

    //마스터 슬라이더 값
    private float _masterValue => Mathf.Pow(10, _soundManager.GetVolumeMaster() / 20f); 
    
    //bgm 슬라이더 값
    private float _bgmSliderValue => _soundManager.GetVolumeBgm() / 20f;
   
    //sfx 슬라이더 값
    private float _sfxSliderValue => _soundManager.GetVolumeSfx() / 20f;
    private float _sensitivity;

    private int _changeDetect => _changeCheck.Count(x => x);
    private Coroutine _changeDetectCo;
    
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _soundManager = SoundManager.Instance; 
        _dataManager = DataManager.Instance;
        
        _refreshRate = Screen.currentResolution.refreshRate;
        
        Bind();
        Init();   
        SetFrameDropDown();
        SetWindowDropdown();
        SetGammaSlider();
        SetSensitivtySlider();
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
        _masterVolumeSlider = GetComponentBind<Slider>("Master_Volume_Slider");
        _sfxVolumeSlider = GetComponentBind<Slider>("SFX_Volume_Slider");
        _bgmVolumeSlider = GetComponentBind<Slider>("BGM_Volume_Slider");

        
        _masterVolumeSlider.value = _masterValue;
        _sfxVolumeSlider.value = _sfxSliderValue;
        _bgmVolumeSlider.value = _bgmSliderValue;
        
        _masterVolumeSlider.onValueChanged.AddListener(x => ChangeMasterVolume(x));
        _sfxVolumeSlider.onValueChanged.AddListener(x => ChangeSfxVolume(x));
        _bgmVolumeSlider.onValueChanged.AddListener(x => ChangeBgmVolume(x));

        
        _cancelButtonText = GetComponentBind<TextMeshProUGUI>("Cancel_Button_Text");
        _confirmButton = GetComponentBind<Button>("Confirm_Button");
        _confirmButton.onClick.AddListener(ChangeSettingSave);
        _resetButton = GetComponentBind<Button>("Reset_Button");
        _resetButton.onClick.AddListener(() =>
        {
            SettingReset.Reset();
            UIReset();
        });
        
        
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
    /// 마스터 볼륨 조절
    /// </summary>
    /// <param name="volume">0.01f ~ 1f</param>
    private void ChangeMasterVolume(float volume)
    {
        _soundManager.SetVolumeMaster(volume);
        _changeCheck[(int)Settings.MASTER_VOLUME] =  !Mathf.Approximately(_soundManager.GetVolumeMaster(), _dataManager.UserSettingData.MasterVolume);
    }
    
    /// <summary>
    /// 배경음 볼륨 조절
    /// </summary>
    /// <param name="volume">0.01f ~ 1f</param>
    private void ChangeBgmVolume(float volume)
    {
        _soundManager.SetVolumeBGM(volume);
        _changeCheck[(int)Settings.BGM_VOLUME] = !(Mathf.Approximately(_soundManager.GetVolumeBgm(), _dataManager.UserSettingData.BgmVolume));
    }

    /// <summary>
    /// 효과음 볼륨 조절
    /// </summary>
    /// <param name="volume">0.01f ~ 1f</param>
    private void ChangeSfxVolume(float volume)
    {
        _soundManager.SetVolumeSFX(volume);
        _changeCheck[(int)Settings.SFX_VOLUME] = !(Mathf.Approximately(_soundManager.GetVolumeSfx(), _dataManager.UserSettingData.SfxVolume));
    }

    private void SetSensitivtySlider()
    {
        _sensitivitySlider = GetComponentBind<Slider>("Sensitivity_Slider");
        _sensitivitySlider.onValueChanged.AddListener(x => ChangeSensitivity(x));
        _sensitivitySlider.value = _dataManager.UserSettingData.Sensitivity; 
    }

    private void ChangeSensitivity(float value)
    {
        _sensitivity = value;
        _changeCheck[(int)Settings.SENSITIVITY] = !(Mathf.Approximately(_sensitivity, _dataManager.UserSettingData.Sensitivity));     
    }
    
    /// <summary>
    /// 감마, 밝기 값 설정
    /// </summary>
    private void SetGammaSlider()
    {
        _gammaSlider = GetComponentBind<Slider>("Gamma_Bright_Slider");
        _gammaSlider.onValueChanged.AddListener(x => ChangeGammaBrightness(x));
        _gammaSlider.value = _dataManager.UserSettingData.GammaBrightness;
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
        _changeCheck[(int)Settings.GAMMA] = !(Mathf.Approximately(_gameManager.CurGammaBrightness, _dataManager.UserSettingData.GammaBrightness));
    }
 
    
    /// <summary>
    /// 프레임 드롭다운 메뉴 설정
    /// </summary>
    private void SetFrameDropDown()
    {
        _frameDropdown = GetComponentBind<TMP_Dropdown>("FrameDropdown");
        _frameDropdown.onValueChanged.AddListener(x => ChangeFrame(x));

        _frameDropdown.value = _dataManager.UserSettingData.Frame;
    }

    /// <summary>
    /// 드롭다운 메뉴 변경 기능
    /// </summary>
    /// <param name="value"></param>
    private void ChangeFrame(int value)
    {
        _gameManager.SetFrames(_frameDict[value], _frameDict[value] == -1);
        _frameDropdownValue = value; 
        _changeCheck[(int)Settings.FRAME] = _frameDropdownValue != _dataManager.UserSettingData.Frame;
    }
    
    
    /// <summary>
    /// 윈도우 모드 드롭다운 메뉴 설정
    /// </summary>
    private void SetWindowDropdown()
    {
        _windowDropdown = GetComponentBind<TMP_Dropdown>("WindowDropdown");
        _windowDropdown.onValueChanged.AddListener(x => ChangeWindowMode(x));

        _windowDropdown.value = _dataManager.UserSettingData.WindowMode;
    }
    
    /// <summary>
    /// 윈도우 모드 설정 드롭다운 변경
    /// </summary>
    /// <param name="value"></param>
    private void ChangeWindowMode(int value)
    {
        _gameManager.SetWindowMode(value);
        _changeCheck[(int)Settings.WINDOWMODE] = _gameManager.CurWindowMode != _dataManager.UserSettingData.WindowMode;
    }

    /// <summary>
    /// 변경 사항 내역 저장
    /// </summary>
    private void ChangeSettingSave()
    {
        _dataManager.UserSettingData.GammaBrightness = _gameManager.CurGammaBrightness;
        _dataManager.UserSettingData.Frame = _frameDropdownValue;
        _dataManager.UserSettingData.Vsync = _gameManager.CurVSyncCount; 
        _dataManager.UserSettingData.MasterVolume = _soundManager.GetVolumeMaster();
        _dataManager.UserSettingData.BgmVolume = _soundManager.GetVolumeBgm();
        _dataManager.UserSettingData.SfxVolume = _soundManager.GetVolumeSfx();
        _dataManager.UserSettingData.WindowMode = _gameManager.CurWindowMode;
        _dataManager.UserSettingData.Sensitivity = _sensitivity;
        CheckSetting();


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

        _frameDropdown.value = _dataManager.UserSettingData.Frame;
        _windowDropdown.value = _dataManager.UserSettingData.WindowMode;
        _gammaSlider.value = _dataManager.UserSettingData.GammaBrightness;
        _masterVolumeSlider.value = Mathf.Pow(10, _dataManager.UserSettingData.MasterVolume / 20f);
        _bgmVolumeSlider.value = _dataManager.UserSettingData.BgmVolume / 20f;
        _sfxVolumeSlider.value = _dataManager.UserSettingData.SfxVolume / 20f;
        _sensitivitySlider.value = _dataManager.UserSettingData.Sensitivity;
        CheckSetting();
        
    }

    /// <summary>
    /// 저장, 취소 시 현재 설정 상태 확인
    /// </summary>
    private void CheckSetting()
    {
        _changeCheck[(int)Settings.MASTER_VOLUME] =  !Mathf.Approximately(_soundManager.GetVolumeMaster(), _dataManager.UserSettingData.MasterVolume);
        _changeCheck[(int)Settings.BGM_VOLUME] = !(Mathf.Approximately(_soundManager.GetVolumeBgm(), _dataManager.UserSettingData.BgmVolume));
        _changeCheck[(int)Settings.SFX_VOLUME] = !(Mathf.Approximately(_soundManager.GetVolumeSfx(), _dataManager.UserSettingData.SfxVolume));
        _changeCheck[(int)Settings.GAMMA] = !(Mathf.Approximately(_gameManager.CurGammaBrightness, _dataManager.UserSettingData.GammaBrightness));
        _changeCheck[(int)Settings.FRAME] = _frameDropdownValue != _dataManager.UserSettingData.Frame;
        _changeCheck[(int)Settings.WINDOWMODE] = _gameManager.CurWindowMode != _dataManager.UserSettingData.WindowMode;
        _changeCheck[(int)Settings.SENSITIVITY] = !(Mathf.Approximately(_sensitivity, _dataManager.UserSettingData.Sensitivity));
    }

    /// <summary>
    /// UI 설정 초기화
    /// </summary>
    private void UIReset()
    {
        _frameDropdown.value = _dataManager.UserSettingData.Frame;
        _windowDropdown.value = _dataManager.UserSettingData.WindowMode;
        _gammaSlider.value = _dataManager.UserSettingData.GammaBrightness;
        _masterVolumeSlider.value = Mathf.Pow(10, _dataManager.UserSettingData.MasterVolume / 20f);
        _bgmVolumeSlider.value = _dataManager.UserSettingData.BgmVolume / 20f;
        _sfxVolumeSlider.value = _dataManager.UserSettingData.SfxVolume / 20f;
        _sensitivitySlider.value = _dataManager.UserSettingData.Sensitivity;
    }
    
}
