using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _itemPickObj;
    
    public static UIManager Instance;
    
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

    /// <summary>
    /// 아이템 픽업 오브젝트 On/Off
    /// </summary>
    /// <param name="isActive"></param>
    public void ItemPickObjActive(bool isActive = false)
    {
        _itemPickObj.SetActive(isActive);
    }
    
    
}
