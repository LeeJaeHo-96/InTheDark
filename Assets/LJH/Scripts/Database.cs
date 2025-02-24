using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using WebSocketSharp;
using TMPro;
using System;

struct ObjectInfo
{
    string objName;
    int objPrice;
}
public class Database : BaseUI
{
    public static Database instance = null;


    [Header("저장할 변수")]
    public int money;
    public int days;
    ObjectInfo obj;

    [Header("텍스트")]
    public TMP_Text dayText;
    public TMP_Text moneyText;

    DatabaseReference database;

    private void Awake()
    {
        SingletonInit();
        Bind();
        Init();
    }

    private void Start()
    {
    }

    /// <summary>
    /// 데이터 저장용 함수 (게임 저장할 때 호출됨)
    /// </summary>
    /// <param name="playerId">아이디</param>
    /// <param name="slot">세이브 슬롯</param>
    /// <param name="money">보유 금액</param>
    /// <param name="day">현재 날짜</param>
    public void SaveData(string playerId, string slot, int money, int day)
    {
        //
        // 유저마다 슬롯이 1 ~ 5 있음
        // 슬롯 1을 선택했을 때, 해당하는 슬롯데이터에 돈과 날짜가 저장되어야 함
        // 플레이어에서 슬롯을 불러오고
        // 슬롯에 따라 해당 데이터를 불러오는 방식
        //
        // 저장할땐 슬롯에 데이터를 저장하고
        // 슬롯을 플레이어에 해당 슬롯 위치에 저장
        // 유저iD를 Key로 슬롯 보관

        //슬롯을 Key로 데이터 보관
        Dictionary<string, object> slotData = new Dictionary<string, object>
    {
        {"Money", money},
        {"Days", day}
    };

        // 슬롯 데이터를 해당 유저의 특정 슬롯 위치에 저장
        database.Child("UserData").Child(playerId).Child(slot)
            .SetValueAsync(slotData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("데이터 저장 실패: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("데이터 저장 성공! 플레이어 ID: " + playerId + " | 슬롯: " + slot);
                }
            });
    }

    /// <summary>
    /// 데이터 불러오기 함수/
    /// 슬롯 버튼에 해당 함수 넣어두고 플레이어 아이디랑 슬롯 넣어주는 식으로 사용
    /// </summary>
    /// <param name="playerId">데이터 불러오기 위한 Key값</param>
    /// <param name="slot">몇번 슬롯인지</param>
    /// 
    public void LoadData(string playerId, string slot)
    {
        database.Child("UserData").Child(playerId).Child(slot)
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("데이터 불러오기 실패: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Debug.Log($"데이터 불러오기 성공! 전체 데이터: {snapshot.GetRawJsonValue()}");

                    // 직접 개별 값 가져오기
                    int money = 150;
                    int days = 1;

                    if (snapshot.HasChild("Money") && snapshot.Child("Money").Value is long moneyValue)
                    {
                        money = (int)moneyValue;
                    }
                    if (snapshot.HasChild("Days") && snapshot.Child("Days").Value is long daysValue)
                    {
                        days = (int)daysValue;
                    }

                    // UI 업데이트
                    moneyText.text = money.ToString();
                    dayText.text = days.ToString();

                    Debug.Log($"데이터 불러오기 성공! 플레이어 ID: {playerId}, 슬롯: {slot}, 보유금: {money}, 날짜: {days}");
                }
                else
                {
                    Debug.Log("저장된 슬롯 데이터가 없습니다. 새로운 슬롯 데이터를 불러옵니다.");
                    moneyText.text = "150";
                    dayText.text = "1";
                }
            }
        });
    }
    public void LoadData1(string playerId, string slot)
    {
        database.Child("UserData").Child(playerId).Child(slot)
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("데이터 불러오기 실패: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot != null)
                {
                    Debug.Log($"데이터 불러오기 성공! 전체 데이터: {snapshot.GetRawJsonValue()}");

                    // Dictionary 변환 시도
                    Dictionary<string, object> slotData = snapshot.Value as Dictionary<string, object>;

                    if (slotData != null)
                    {
                        // Money 값 가져오기
                        int money = slotData.ContainsKey("Money") && slotData["Money"] is long moneyValue ? (int)moneyValue : 150;

                        // Days 값 가져오기
                        int days = slotData.ContainsKey("Days") && slotData["Days"] is long daysValue ? (int)daysValue : 1;

                        // UI 업데이트
                        moneyText.text = money.ToString();
                        dayText.text = days.ToString();

                        Debug.Log($"데이터 불러오기 성공! 플레이어 ID: {playerId}, 슬롯: {slot}, 보유금: {money}, 날짜: {days}");
                    }
                    else
                    {
                        Debug.Log("slotData 변환 실패! 기본값으로 설정");
                        moneyText.text = "150";
                        dayText.text = "1";
                    }
                }
                else
                {
                    Debug.Log("저장된 슬롯 데이터가 없습니다. 새로운 슬롯 데이터를 불러옵니다.");
                    moneyText.text = "150";
                    dayText.text = "1";
                }
            }
        });
    }

    void Init()
    {

        database = FirebaseDatabase.DefaultInstance.RootReference;


        if (dayText != null)
            if (moneyText != null)
            {
                dayText.text = "1";
                moneyText.text = "150";
            }
    }

    void SingletonInit()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
