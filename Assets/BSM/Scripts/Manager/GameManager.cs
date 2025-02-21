using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ExitGames.Client.Photon;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        StartFrame(); 
    }


    private void Update()
    {
        Debug.Log($"frame :{Application.targetFrameRate} / Vsync:{QualitySettings.vSyncCount}");
    }

    //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
 
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
    
}
