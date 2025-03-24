using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInit : BaseUI
{
    //  테스트용 스크립트임
    //
    //
    //
    //
    //

    [Header("텍스트")]
    public TMP_Text dayText;
    public TMP_Text moneyText;

    [Header("버튼")]
    public Button exitButton;
    public Button PlusDayButton;
    public Button PlusMoneyButton;

    void Awake()
    {
        Bind();
        Init();
    }

    private void Start()
    {
        //테스트 코드
        //Database.instance.dayText = dayText;
        //Database.instance.moneyText = moneyText;
    }

    void ExitButton()
    {
#if UNITY_EDITOR
        //Comment : 유니티 에디터상에서 종료
        Database.instance.SaveData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_1", int.Parse(moneyText.text), int.Parse(dayText.text));
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //Comment : 빌드 상에서 종료
        Application.Quit();
#endif
    }

    void PlusDay()
    {
            //Database.instance.days += 1;
            //dayText.text = Database.instance.days.ToString();
    }
    void PlusMoney()
    {
            //Database.instance.money += 100;
            //moneyText.text = Database.instance.money.ToString();
    }
    void Init()
    {
        dayText = GetUI<TMP_Text>("Day");
        moneyText = GetUI<TMP_Text>("Money");

        exitButton = GetUI<Button>("ExitButton");
        exitButton.onClick.AddListener(ExitButton);

        PlusDayButton = GetUI<Button>("PlusDay");
        PlusMoneyButton = GetUI<Button>("PlusMoney");

        PlusDayButton.onClick.AddListener(PlusDay);
        PlusMoneyButton.onClick.AddListener(PlusMoney);
    }
}
