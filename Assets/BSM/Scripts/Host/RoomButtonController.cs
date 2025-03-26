using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RoomButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    
    private RoomButton _curRoomButton;
    private Image _curButtonColor;
    private Image _refreshImageColor;
    private TextMeshProUGUI _curButtonTextColor;
        
    private Button[] _commonButtons;
    
    private Color _activateButtonColor = new Color(0.98f, 0.09f, 0.26f);
    private Color _deActivateTextColor = new Color(0.98f, 0.09f, 0.26f);
    
    private void Awake() => Init();
    
    private void Init()
    {
        _commonButtons = GetComponentsInChildren<Button>(true);

        _curRoomButton = _commonButtons[0].GetComponent<RoomButton>();
        
        _curButtonColor = _curRoomButton.GetComponent<Image>();
        _curButtonTextColor = _curRoomButton.GetComponentInChildren<TextMeshProUGUI>();
        OnClickAddListener();
    }
    
    private void OnClickAddListener()
    { 
        _commonButtons[1].onClick.AddListener(() => _panel.SetActive(false));
        
    }
    
    /// <summary>
    /// 버튼 색상 변경
    /// </summary>
    /// <param name="commonButton">감지한 버튼</param>
    /// <param name="buttonColor">변경할 버튼의 이미지 오브젝트</param>
    /// <param name="buttonTextColor">변경할 버튼의 텍스트 오브젝트</param>
    public void CurrentButtonChanged(RoomButton commonButton, Image buttonColor, TextMeshProUGUI buttonTextColor)  
    {
        if (_curRoomButton == commonButton) return;

        //이전 버튼의 색상 변경
        _curButtonColor.color = Color.white;
        _curButtonTextColor.color = _deActivateTextColor;
          
        //새로 감지한 버튼의 색상 변경
        _curRoomButton = commonButton;
        _curButtonColor = buttonColor; 
        _curButtonTextColor = buttonTextColor;
        
        _curButtonColor.color = _activateButtonColor;
        _curButtonTextColor.color = Color.white; 
    }
}
