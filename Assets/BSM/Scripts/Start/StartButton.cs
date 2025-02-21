using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler
{
    private Image _buttonColor;
    private TextMeshProUGUI _buttonTextColor;

    private StartButtonController _startButtonController;
    
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
 
}
