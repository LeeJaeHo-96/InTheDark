using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    TMP_Text moneyText;
    private void Start()
    {
        moneyText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        moneyText.text = $"º¸À¯±Ý : {IngameManager.Instance.money}";
    }
}
