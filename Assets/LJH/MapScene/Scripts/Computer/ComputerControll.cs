using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

enum Text
{
    menu = 1,
    land,
    store,
    check
}
public class ComputerControll : BaseUI
{
    const string land = "목적지 선택";
    const string store = "상점 선택";
    const string land_Start = "시작의 섬";
    const string land_Middle = "중간섬";
    const string land_End = "끝의 섬";
    const string flashlight = "손전등";
    const string stick = "막대기";
    const string exit = "나가기";
    const string and = "계속";
    const string buy = "완료";

    TMP_Text menuText;
    TMP_Text landText;
    TMP_Text storeText;
    TMP_Text checkText;

    List<GameObject> textObjList = new List<GameObject>();
    GameObject curPage;

    TMP_InputField inputField;

    //아이템 구매용 리스트
    List<Item> items = new List<Item>();
    float itemsPrice;

    [Header("아이템 프리팹 넣어두는 리스트")]
    [SerializeField] List<Item> itemList = new List<Item>();
    [Header("열기구 생성 위치")]
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject destinationPoint;


    private void Awake()
    {
        Bind();
        Init();
    }


    private void OnEnable()
    {
        TextSetActive((int)Text.menu);

        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();


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
                            inputField.ActivateInputField();
                            break;

                        case store:
                            TextSetActive((int)Text.store);
                            //상점 열때마다 아이템 목록 초기화
                            items.Clear();
                            inputField.ActivateInputField();
                            break;

                        case exit:
                            gameObject.SetActive(false);
                            break;

                        default:
                            inputField.text = "";
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요_Menu");
                            inputField.ActivateInputField();
                            break;
                    }
                    break;

                case "SubMenuText_Land":
                    switch (inputText)
                    {
                        case land_Start:
                            //Todo : 목적지 시작의섬 선택
                            inputField.ActivateInputField();
                            break;

                        case land_Middle:
                            //Todo : 목적지 중간섬 선택
                            inputField.ActivateInputField();
                            break;

                        case land_End:
                            //Todo : 목적지 끝의 섬 선택
                            inputField.ActivateInputField();
                            break;

                        case exit:
                            TextSetActive((int)Text.menu);
                            inputField.ActivateInputField();
                            break;

                        default:
                            inputField.text = "";
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요");
                            inputField.ActivateInputField();
                            break;
                    }
                    break;

                case "SubMenuText_Store":
                    switch (inputText)
                    {
                        case flashlight:
                            //Todo : 구매 리스트에 손전등 추가
                            AddItemList(itemList[0]);
                            inputField.ActivateInputField();
                            break;

                        case stick:
                            //Todo : 구매 리스트에 막대기 추가
                            AddItemList(itemList[1]);
                            inputField.ActivateInputField();
                            break;

                        case exit:
                            items.Clear();
                            TextSetActive((int)Text.menu);
                            inputField.ActivateInputField();
                            break;

                        default:
                            inputField.text = "";
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요");
                            inputField.ActivateInputField();
                            break;
                    }
                    break;

                case "SubMenuText_Check":
                    switch (inputText)
                    {
                        case and:
                            //Todo : 구매 리스트에 손전등 추가
                            if (itemsPrice >= IngameManager.Instance.money)
                            {
                                Debug.Log("보유금이 부족하여 '계속'을 진행할 수 없습니다.");
                                TextSetActive((int)Text.check);
                            }
                            else
                            {
                                TextSetActive((int)Text.store);
                            }
                            //이후에 추가 구매? 구매 완료? 화면 넣어야함
                            inputField.ActivateInputField();
                            break;

                        case buy:
                            //구매 완료
                            CallAirBalloon();
                            gameObject.SetActive(false);
                            break;

                        default:
                            inputField.text = "";
                            //Todo 플레이스홀더 내용 바꿔야할듯?
                            Debug.Log("다시 입력해주세요");
                            inputField.ActivateInputField();
                            break;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 쇼핑
    /// </summary>
    /// <param name="item"></param>
    void AddItemList(Item item)
    {
       ////머지 이후에 주석해제하여 테스트
       if(itemPriceCheck(item))
       {
           items.Add(item);
           TextSetActive((int)Text.check);
        }
       else
       {
            TextSetActive((int)Text.store);
            //Todo :돈부족 텍스트 노출
            Debug.Log("보유금이 부족합니다.");
       }
        inputField.text = "";
    }

    bool itemPriceCheck(Item item)
    {
        InteractableItemData itemData;
        Debug.Log(item.name);

        switch (item.name)
        {
            case "FlashLight":
                itemData = ItemManager.Instance.GetItemData(4); // 손전등 Id
                break;

            case "FishingRod":
                itemData = ItemManager.Instance.GetItemData(2); // 낚싯대 Id
                break;

            default:
                itemData = null;
                return false;
        }

        Debug.Log($"아이템 데이터 {itemData.name}");
        float itemPrice = itemData.ItemBuyPrice;
        Debug.Log($"{item.name}의 가격 {itemPrice}");

        if(itemsPrice + itemPrice > IngameManager.Instance.money)
        { 
            return false;
        }
        
        itemsPrice += itemPrice;
        Debug.Log($"담은 아이템의 가격 {itemsPrice}1");
        return true;
    }

    /// <summary>
    /// 컴퓨터에서 물건 구매 완료시 해당 함수 호출하여 열기구 생성
    /// </summary>
    public void CallAirBalloon()
    {
        Debug.Log($"담은 아이템의 가격 {itemsPrice}2");
        IngameManager.Instance.money -= itemsPrice;
        Debug.Log($"변동된 돈 {IngameManager.Instance.money}");

        GameObject airBallonPrefab = PhotonNetwork.Instantiate("HotAirBalloon", spawnPoint.transform.position, Quaternion.identity);
        airBallonPrefab.GetComponent<HotAirBalloon>().spawnPoint = spawnPoint;
        airBallonPrefab.GetComponent<HotAirBalloon>().destinationPoint = destinationPoint;
        airBallonPrefab.transform.GetChild(0).GetComponent<Basket>().itemList = items;
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
        checkText = GetUI<TMP_Text>("SubMenuText_Check");

        textObjList.Add(menuText.gameObject);
        textObjList.Add(landText.gameObject);
        textObjList.Add(storeText.gameObject);
        textObjList.Add(checkText.gameObject);

        curPage = textObjList[0];

        inputField = GetUI<TMP_InputField>("InputField (TMP)");
    }
}
