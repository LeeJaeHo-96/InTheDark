using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    TMP_Text dayText;
    private void Start()
    {
        dayText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        dayText.text = $"{IngameManager.Instance.days}ÀÏÂ÷";
    }
}
