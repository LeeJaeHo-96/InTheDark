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

    const int DefaultMoney = 250;
    const int DefaultDays = 0;

    [Header("텍스트")]
    //public TMP_Text dayText;
    //public TMP_Text moneyText;

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
        database.Child("UserData").Child(playerId).Child(slot.ToString())
            .SetValueAsync(slotData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("데이터 저장 실패: " + task.Exception);
                }
            });
    }

    public void ResetData(string playerId, string slot)
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
        {"Money", DefaultMoney},
        {"Days", DefaultDays}
    };



        // 슬롯 데이터를 해당 유저의 특정 슬롯 위치에 저장
        database.Child("UserData").Child(playerId).Child(slot.ToString())
            .SetValueAsync(slotData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("데이터 저장 실패: " + task.Exception);
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

        database.Child("UserData").Child(playerId).Child(slot.ToString())
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

                Debug.Log($"snapshot.Exists: {snapshot.Exists}");

                if (snapshot.Exists)
                {
                    if (snapshot.HasChild("Money"))
                    {
                        object moneyValue = snapshot.Child("Money").Value;
                        if (moneyValue != null)
                        {
                            IngameManager.Instance.money = Convert.ToInt32(moneyValue);
                        }
                    }
                    else
                    {
                        IngameManager.Instance.money = DefaultMoney;
                    }


                    if (snapshot.HasChild("Days"))
                    {
                        object daysValue = snapshot.Child("Days").Value;
                        if (daysValue != null)
                        {
                            IngameManager.Instance.days = Convert.ToInt32(daysValue);
                        }
                    }
                    else
                    {
                        IngameManager.Instance.days = DefaultDays;
                    }

                }
                else
                {
                    Debug.Log("저장된 슬롯 데이터가 없습니다. 새로운 슬롯 데이터를 불러옵니다.");
                    //테스트 코드
                    //moneyText.text = "150";
                    //dayText.text = "1";

                    IngameManager.Instance.money = 250;
                    IngameManager.Instance.days = 1;

                }
            }
        });
    }

    void Init()
    {

        database = FirebaseDatabase.DefaultInstance.RootReference;

       //테스트 코드
       // if (dayText != null)
       //     if (moneyText != null)
       //     {
       //         dayText.text = "1";
       //         moneyText.text = "150";
       //     }
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
