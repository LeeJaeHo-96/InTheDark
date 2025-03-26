using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RoomButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Image _buttonColor;
    private TextMeshProUGUI _buttonTextColor;

    private RoomButtonController _roomButtonController;
    private SoundManager _soundManager => SoundManager.Instance;
    
    private void Awake() => Init();
    
    private void Init()
    {
        _buttonColor = GetComponent<Image>();
        _buttonTextColor = GetComponentInChildren<TextMeshProUGUI>();

        _roomButtonController = GetComponentInParent<RoomButtonController>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _roomButtonController.CurrentButtonChanged(this, _buttonColor, _buttonTextColor);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //버튼 클릭 SFX
        _soundManager.PlaySfx(_soundManager.SoundDatas.SoundDict["ButtonClickSFX"]);
    }
} 