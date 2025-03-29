using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlotTest : MonoBehaviour
{
    [SerializeField] List<Button> buttonList = new List<Button>();

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        foreach (var button in buttonList)
        {
            button.GetComponentInChildren<TMP_Text>().text = "비어있음";
        }
    }


    void Init()
    {
        // 테스트용 코드
        // Todo : 더 깔끔하게 줄여서 사용해야함
        buttonList[0].onClick.AddListener(() => Database.instance.LoadData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_1"));
        buttonList[1].onClick.AddListener(() => Database.instance.LoadData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_2"));
        buttonList[2].onClick.AddListener(() => Database.instance.LoadData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_3"));
        buttonList[3].onClick.AddListener(() => Database.instance.LoadData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_4"));
        buttonList[4].onClick.AddListener(() => Database.instance.LoadData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_5"));

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(() => SceneManager.LoadScene("DataBaseScene"));
        }
    }
}
