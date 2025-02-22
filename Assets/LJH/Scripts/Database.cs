using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using WebSocketSharp;
using TMPro;

struct ObjectInfo
{
    string objName;
    int objPrice;
}
public class Database : MonoBehaviour
{
    [Header("저장할 변수")]
    int money;
    int days;
    ObjectInfo obj;

    [Header("텍스트")]
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text moneyText;

    DatabaseReference database;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        //SaveData("1번플레이어", 1, 100, 1);
        LoadData("1번플레이어", 1);
    }

    /// <summary>
    /// 데이터 저장용 함수 (게임 저장할 때 호출됨)
    /// </summary>
    /// <param name="playerId">아이디</param>
    /// <param name="slot">세이브 슬롯</param>
    /// <param name="money">보유 금액</param>
    /// <param name="day">현재 날짜</param>
    void SaveData(string playerId, int slot, int money, int day)
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
        {"보유금", money},
        {"현재 날짜", day}
    };

        // 슬롯 데이터를 해당 유저의 특정 슬롯 위치에 저장
        database.Child("Players").Child(playerId).Child("Slots").Child("slot_" + slot)
            .SetValueAsync(slotData)
            .ContinueWith(task =>
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
    void LoadData(string playerId, int slot)
    {
        database.Child("Players").Child(playerId).Child("Slots").Child("slot_" + slot)
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
                    Dictionary<string, object> slotData = snapshot.Value as Dictionary<string, object>;

                    // money와 day 값 가져오기
                    int money = int.Parse(slotData["보유금"].ToString());
                    int day = int.Parse(slotData["현재 날짜"].ToString());

                    moneyText.text = money + "원";
                    dayText.text = day + "일차";

                    Debug.Log($"데이터 불러오기 성공! 플레이어 ID: {playerId}, 슬롯: {slot}, 보유금: {money}, 날짜: {day}");
                }
                else
                {
                    Debug.Log("저장된 슬롯 데이터가 없습니다.");
                }
            }
        });
    }

   


    void Init()
    {

        database = FirebaseDatabase.DefaultInstance.RootReference;

        dayText.text = "0일차";
        moneyText.text = "0원";
    }




}
