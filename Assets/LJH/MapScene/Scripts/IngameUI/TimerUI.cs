using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    TMP_Text timerText;

    private void OnEnable()
    {
        IngameManager.Instance.TimerReset();
        StartCoroutine(IngameManager.Instance.TimerCount());
    }
    private void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        timerText.text = $"{IngameManager.Instance.time} : {IngameManager.Instance.minute}";
    }
}
