using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ExitGames.Client.Photon;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    public ColorGrading PostVolume;

    private PostProcessProfile _postProfile;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
        StartFrame();
        StartWindowMode();
        StartGammaBrightness();
    }

    
    /// <summary>
    /// 게임 매니저 초기화 작업
    /// </summary>
    private void Init()
    { 
        _postProfile = transform.GetChild(0).GetComponent<PostProcessVolume>().profile;
        _postProfile.TryGetSettings<ColorGrading>(out PostVolume);
    }
    
    /// <summary>
    /// 게임 시작 시 설정한 프레임으로 변경
    /// </summary>
    private void StartFrame()
    {
        //프레임을 설정했었는지 확인
        if (PlayerPrefs.HasKey("FrameRate"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("FrameRate");
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync");
        }
        else
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            QualitySettings.vSyncCount = 0;
        } 
    }
    
    /// <summary>
    /// 게임 시작 시 설정한 윈도우 모드로 변경
    /// </summary>
    private void StartWindowMode()
    {
        if (PlayerPrefs.HasKey("WindowDropdownValue"))
        {
            Screen.fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt("WindowDropdownValue");
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;  
        }
    }
    
    /// <summary>
    /// 게임 시작 시 감마, 밝기 설정
    /// </summary>
    private void StartGammaBrightness()
    {
        if (PlayerPrefs.HasKey("GammaBrightnessValue"))
        {
            float gammaValue = PlayerPrefs.GetFloat("GammaBrightnessValue");
            PostVolume.postExposure.value = gammaValue;
            PostVolume.gamma.value = new Vector4(gammaValue, gammaValue, gammaValue, gammaValue);
        }
        else
        {
            PostVolume.postExposure.value = 0f;
            PostVolume.gamma.value = new Vector4(1f, 1f, 1f, 1f);
        }
    }
    
    /// <summary>
    /// 전달 받은 프레임 옵션으로 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetFrames(int value, bool force = false)
    {
        PlayerPrefs.SetInt("FrameRate", value);
        Application.targetFrameRate = value; 
        QualitySettings.vSyncCount = force ? 1 : 0;
        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);

    }
    
    /// <summary>
    /// 전달 받은 값으로 윈도우 모드 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetWindowMode(int value)
    {
        Screen.fullScreenMode = (FullScreenMode)value; 
    }
    
    /// <summary>
    /// 전달 받은 값으로 감마, 밝기 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetGammaBrightness(float value)
    {
        PostVolume.postExposure.value = value;
        PostVolume.gamma.value = new Vector4(value, value, value, value);
    }

}
