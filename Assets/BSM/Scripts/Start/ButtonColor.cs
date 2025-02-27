using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image _buttonColor;
    private TextMeshProUGUI _buttonTextColor;

    private void Awake() => Init();
    
    private void Init()
    {
        _buttonColor = GetComponent<Image>();
        _buttonTextColor = GetComponentInChildren<TextMeshProUGUI>();

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonColor.color = new Color(1f, 0.5f, 0f);
        _buttonTextColor.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonColor.color = Color.white;
        _buttonTextColor.color = new Color(0.26f, 0.26f, 0.26f);
    }
}
