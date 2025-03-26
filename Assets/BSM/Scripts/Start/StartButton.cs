using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Image _buttonColor;
    private TextMeshProUGUI _buttonTextColor;

    private StartButtonController _startButtonController;
    private SoundManager _soundManager => SoundManager.Instance;
    
    private void Awake() => Init();
    
    private void Init()
    {
        _buttonColor = GetComponent<Image>();
        _buttonTextColor = GetComponentInChildren<TextMeshProUGUI>();

        _startButtonController = GetComponentInParent<StartButtonController>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _startButtonController.CurrentButtonChanged(this, _buttonColor, _buttonTextColor);

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //버튼 클릭 SFX
        _soundManager.PlaySfx(_soundManager.SoundDatas.SoundDict["ButtonClickSFX"]);
    }
}
