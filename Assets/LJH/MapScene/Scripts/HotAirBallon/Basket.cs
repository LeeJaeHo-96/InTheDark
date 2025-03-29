using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviourPun, IHitMe
{
    public List<Item> itemList = new List<Item>();

    HotAirBalloon balloon;

    public bool HitMe { get; set; }
    PopUp popUp;

    void Start()
    {
        popUp = GetComponent<PopUp>();
        balloon = transform.parent.GetComponent<HotAirBalloon>();
    }

    private void Update()
    {

        if(popUp.HitMe && Input.GetKeyDown(KeyCode.E))
        {
            ShowItem(itemList);
        }
    }

    /// <summary>
    /// 컴퓨터에서 해당 함수 호출하여 아이템 리스트에 아이템 정보 담아줌
    /// </summary>
    /// <param name="itemName"></param>
    public void BuyItem(List<Item> itemList, Item itemName)
    {
        itemList.Add(itemName);
        this.itemList = itemList;
        //Todo : 물건 사면 돈 소모
        //GameManager.Instance.money -= itemName.price;
    }

    /// <summary>
    /// 담아준 아이템을 뿌려주는 함수
    /// </summary>
    /// <param name="itemList"></param>
    void ShowItem(List<Item> itemList)
    {
        //구매한 아이템을 리스트에 넣어주고 그 아이템들을 바스켓을 눌렀을 때, 주변에 뿌려야함
        for (int i = 0; i <= itemList.Count; i++)
        {
            if (i == itemList.Count)
            {
                StopCoroutine(balloon.boomCo);
                StartCoroutine(balloon.BoomAirBalloon(1f));
            }
            else
            {
                Vector3 itemPos = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                string itemName = itemList[i].name;

                PhotonNetwork.Instantiate(itemName, itemPos, Quaternion.identity);
            }
        }
    }
}
