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
    public int CurFrame;
    public int CurVSyncCount;
    public int CurWindowMode;
    public float CurGammaBrightness;
    
    private ColorGrading PostVolume; 
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
            CurFrame = PlayerPrefs.GetInt("FrameRate");
            CurVSyncCount = PlayerPrefs.GetInt("Vsync");
            
            Application.targetFrameRate = CurFrame;
            QualitySettings.vSyncCount = CurVSyncCount;
        }
        else
        {
            CurFrame = Screen.currentResolution.refreshRate;
            CurVSyncCount = 0;

            Application.targetFrameRate = CurFrame;
            QualitySettings.vSyncCount = CurVSyncCount;
            
            PlayerPrefs.SetInt("FrameDropdownValue", 0);
            PlayerPrefs.SetInt("FrameRate", CurFrame);
            PlayerPrefs.SetInt("Vsync", CurVSyncCount);
        } 
    }
    
    /// <summary>
    /// 게임 시작 시 설정한 윈도우 모드로 변경
    /// </summary>
    private void StartWindowMode()
    {
        if (PlayerPrefs.HasKey("WindowMode"))
        {
            CurWindowMode = PlayerPrefs.GetInt("WindowMode"); 
            Screen.fullScreenMode = (FullScreenMode)CurWindowMode;
        }
        else
        {
            CurWindowMode = 0; 
            Screen.fullScreenMode = (FullScreenMode)CurWindowMode; 
            
            PlayerPrefs.SetInt("WindowMode", CurWindowMode);
        }
    }
    
    /// <summary>
    /// 게임 시작 시 감마, 밝기 설정
    /// </summary>
    private void StartGammaBrightness()
    {
        if (PlayerPrefs.HasKey("GammaBrightness"))
        {
            CurGammaBrightness = PlayerPrefs.GetFloat("GammaBrightness");
            PostVolume.postExposure.value = CurGammaBrightness;
            PostVolume.gamma.value = new Vector4(CurGammaBrightness, CurGammaBrightness, CurGammaBrightness, CurGammaBrightness);
        }
        else
        {
            PostVolume.postExposure.value = 0f;
            PostVolume.gamma.value = new Vector4(1f, 1f, 1f);
            PlayerPrefs.SetFloat("GammaBrightness", 0f);
        }
    }
    
    /// <summary>
    /// 전달 받은 프레임 옵션으로 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetFrames(int value, bool force = false)
    {
        CurFrame = value;
        CurVSyncCount = force ? 1 : 0;
        
        Application.targetFrameRate = value; 
        QualitySettings.vSyncCount = CurVSyncCount; 
    }
    
    /// <summary>
    /// 전달 받은 값으로 윈도우 모드 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetWindowMode(int value)
    {
        CurWindowMode = value;
        
        Screen.fullScreenMode = (FullScreenMode)CurWindowMode; 
    }
    
    /// <summary>
    /// 전달 받은 값으로 감마, 밝기 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetGammaBrightness(float value)
    {
        CurGammaBrightness = value;
        
        PostVolume.postExposure.value = CurGammaBrightness;
        PostVolume.gamma.value = new Vector4(CurGammaBrightness, CurGammaBrightness, CurGammaBrightness, CurGammaBrightness);
    }

}
