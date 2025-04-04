using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

[CreateAssetMenu(menuName = "Data/UserSettingData")]
public class SettingData : ScriptableObject
{
    public float MasterVolume = -20f;
    public float BgmVolume = 5f;
    public float SfxVolume = 5f;
    public float GammaBrightness;
    public float Sensitivity = 0.01f;
    
    public int Vsync;
    public int WindowMode;
    public int Frame;
    
    
}

public static class SettingReset
{
    /// <summary>
    /// Data 값을 초기화
    /// </summary>
    public static void Reset()
    {
        DataManager.Instance.UserSettingData.MasterVolume = -20f;
        DataManager.Instance.UserSettingData.SfxVolume = 5f;
        DataManager.Instance.UserSettingData.BgmVolume = 5f;
        DataManager.Instance.UserSettingData.GammaBrightness = 0f;
        DataManager.Instance.UserSettingData.Vsync = 0;
        DataManager.Instance.UserSettingData.WindowMode = 0;
        DataManager.Instance.UserSettingData.Frame = 0;
        DataManager.Instance.UserSettingData.Sensitivity = 0.01f;
    } 
}