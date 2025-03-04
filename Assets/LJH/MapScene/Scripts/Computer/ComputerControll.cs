using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

enum Text
{
    menu = 1,
    land,
    store
}
public class ComputerControll : BaseUI
{
    const string land = "목적지 선택";
    const string store = "상점 선택";
    const string land_Start = "시작의 섬";
    const string land_Middle = "중간섬";
    const string land_End = "끝의 섬";
    const string light = "손전등";
    const string stick = "막대기";
    const string exit = "나가기";

    TMP_Text menuText;
    TMP_Text landText;
    TMP_Text storeText;

    List<GameObject> textObjList = new List<GameObject>();
    GameObject curPage;

    TMP_InputField inputField;

    private void Awake()
    {
        Bind();
        Init();
    }

    private void OnEnable()
    {
        TextSetActive((int)Text.menu);

        inputField.gameObject.SetActive(true);
    }

    private void Update()
    {
        TextInput(inputField.text);
    }

    /// <summary>
    /// 글자 입력
    /// </summary>
    /// <param name="inputText"></param>
    void TextInput(string inputText)
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (curPage.name)
            {
                //메뉴 페이지일 때
                case "MenuText":
                    switch (inputText)
                    {
                        case land:
                            TextSetActive((int)Text.land);
                            break;

                        case store:
                            TextSetActive((int)Text.store);
                            break;

                        case exit:
                            gameObject.SetActive(false);
                            break;

                        default:
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요_Menu");
                            break;
                    }
                    break;

                case "SubMenuText_Land":
                    switch (inputText)
                    {
                        case land_Start:
                            //Todo : 목적지 시작의섬 선택
                            Debug.Log("시작의 섬 으로 설정");
                            break;

                        case land_Middle:
                            //Todo : 목적지 중간섬 선택
                            Debug.Log("중간섬으로 설정");
                            break;

                        case land_End:
                            //Todo : 목적지 끝의 섬 선택
                            Debug.Log("끝의 섬으로 설정");
                            break;

                        case exit:
                            TextSetActive((int)Text.menu);
                            break;

                        default:
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요");
                            break;
                    }
                    break;

                case "SubMenuText_Store":
                    switch (inputText)
                    {
                        case light:
                            //Todo : 구매 리스트에 손전등 추가
                            Debug.Log("손전등 추가");
                            break;

                        case stick:
                            //Todo : 구매 리스트에 막대기 추가
                            Debug.Log("막대기 추가");
                            break;

                        case exit:
                            TextSetActive((int)Text.menu);
                            break;

                        default:
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요");
                            break;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 텍스트 활성화/비활성화 제어
    /// </summary>
    /// <param name="index">1 = menu, 2 = land, 3 = store</param>
    void TextSetActive(int index)
    {
        foreach (GameObject go in textObjList)
        {
            go.SetActive(false);
        }
        textObjList[index - 1].gameObject.SetActive(true);
        curPage = textObjList[index - 1];
        inputField.text = "";
    }

    void Init()
    {
        menuText = GetUI<TMP_Text>("MenuText");
        landText = GetUI<TMP_Text>("SubMenuText_Land");
        storeText = GetUI<TMP_Text>("SubMenuText_Store");

        textObjList.Add(menuText.gameObject);
        textObjList.Add(landText.gameObject);
        textObjList.Add(storeText.gameObject);

        curPage = textObjList[0];

        inputField = GetUI<TMP_InputField>("InputField (TMP)");
    }
}
