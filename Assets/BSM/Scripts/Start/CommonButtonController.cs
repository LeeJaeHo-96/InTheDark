using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CommonButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    
    private CommonButton _curCommonButton;
    private Image _curButtonColor;
    private TextMeshProUGUI _curButtonTextColor;
        
    private Button[] _commonButtons;
    
    private void Awake() => Init();
    
    private void Init()
    {
        _commonButtons = GetComponentsInChildren<Button>(true);

        _curCommonButton = _commonButtons[0].GetComponent<CommonButton>();
        
        _curButtonColor = _curCommonButton.GetComponent<Image>();
        _curButtonTextColor = _curCommonButton.GetComponentInChildren<TextMeshProUGUI>();
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
    public void CurrentButtonChanged(CommonButton commonButton, Image buttonColor, TextMeshProUGUI buttonTextColor)
    {
        if (_curCommonButton == commonButton) return;

        //이전 버튼의 색상 변경
        _curButtonColor.color = Color.black;
        _curButtonTextColor.color =  new Color(0.98f, 0.07f, 0.26f);
          
        //새로 감지한 버튼의 색상 변경
        _curCommonButton = commonButton;
        _curButtonColor = buttonColor; 
        _curButtonTextColor = buttonTextColor;
        
        _curButtonColor.color = new Color(1f, 0.5f, 0f);
        _curButtonTextColor.color = Color.black; 
    }
}
