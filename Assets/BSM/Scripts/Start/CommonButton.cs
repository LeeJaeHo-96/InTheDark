using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Image _buttonColor;
    private TextMeshProUGUI _buttonTextColor;

    private CommonButtonController _commonButtonController;
    private SoundManager _soundManager => SoundManager.Instance;
    
    private void Awake() => Init();
    
    private void Init()
    {
        _buttonColor = GetComponent<Image>();
        _buttonTextColor = GetComponentInChildren<TextMeshProUGUI>();

        _commonButtonController = GetComponentInParent<CommonButtonController>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _commonButtonController.CurrentButtonChanged(this, _buttonColor, _buttonTextColor);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //버튼 클릭 SFX
        _soundManager.PlaySfx(_soundManager.SoundDatas.SoundDict["ButtonClickSFX"]);
    }
}
