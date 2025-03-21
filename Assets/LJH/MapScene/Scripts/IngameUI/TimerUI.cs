using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    TMP_Text timerText;

    private void OnEnable()
    {
        //시작할 때, 타이머 리셋 후 타이머 시작됨
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
