using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

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

    DatabaseReference database;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SaveData("1번플레이어", 1, 100, 1);
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

        List<Dictionary<string, object>> slots = new List<Dictionary<string, object>>();

        //슬롯을 Key로 데이터 보관
        Dictionary<string, object> slotData = new Dictionary<string, object>
        {
            {"보유금", money},
            {"현재 날짜", day}
        };

        if (database == null)
        {
            Debug.Log("데이터베이스비어있어");
        }
        database.Child("PlayerID").SetValueAsync(playerId);
        database.Child(playerId).Child("Slot").SetValueAsync(slots);
    }

    /// <summary>
    /// 데이터 불러오기 함수
    /// </summary>
    void LoadData()
    {

    }


    void Init()
    {
        
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }
    

    

}
