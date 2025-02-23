using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInit : BaseUI
{

    [Header("≈ÿΩ∫∆Æ")]
    public TMP_Text dayText;
    public TMP_Text moneyText;

    void Awake()
    {
        Bind();
        Init();
    }

    private void Start()
    {
        Database.instance.dayText = dayText;
        Database.instance.moneyText = moneyText;
    }

    void Init()
    {
        dayText = GetUI<TMP_Text>("Day");
        moneyText = GetUI<TMP_Text>("Money");
    }
}
