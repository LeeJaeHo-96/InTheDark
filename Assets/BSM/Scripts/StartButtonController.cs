using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{

    private StartButton _curStartButton;
    private Image _curButtonColor;
    private TextMeshProUGUI _curButtonTextColor;

    private Button[] _startButtons;

    private void Awake() => Init();
    
    private void Init()
    {
        _startButtons = GetComponentsInChildren<Button>();
        
        _curStartButton = _startButtons[0].GetComponent<StartButton>();
        _curButtonColor = _curStartButton.GetComponent<Image>();
        _curButtonTextColor = _curStartButton.GetComponentInChildren<TextMeshProUGUI>();

        OnClickEventRegister();
    }

    private void OnClickEventRegister()
    {
        
        
        _startButtons[4].onClick.AddListener(ExitGame);
    }
    
    
    /// <summary>
    /// 버튼 색상 변경
    /// </summary>
    /// <param name="startButton">감지한 버튼</param>
    /// <param name="buttonColor">변경할 버튼의 이미지 오브젝트</param>
    /// <param name="buttonTextColor">변경할 버튼의 텍스트 오브젝트</param>
    public void CurrentButtonChanged(StartButton startButton, Image buttonColor, TextMeshProUGUI buttonTextColor)
    {
        if (_curStartButton == startButton) return;

        //이전 버튼의 색상 변경
        _curButtonColor.color = Color.black;
        _curButtonTextColor.color =  new Color(0.98f, 0.07f, 0.26f);
          
        //새로 감지한 버튼의 색상 변경
        _curStartButton = startButton;
        _curButtonColor = buttonColor; 
        _curButtonTextColor = buttonTextColor;
        
        _curButtonColor.color = new Color(1f, 0.5f, 0f);
        _curButtonTextColor.color = Color.black; 
    }

    
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();   
#endif


    }
    
}
