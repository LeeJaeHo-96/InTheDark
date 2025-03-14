using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //TODO: 테스트용
    public List<PlayerController> PlayerObjects = new List<PlayerController>();
    
    public static DataManager Instance;

    public SettingData UserSettingData;
 
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
    }
 
}
