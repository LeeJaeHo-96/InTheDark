using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSave : MonoBehaviour
{
    List<GameObject> items = new List<GameObject>();
    
    Dictionary<int, Vector3> posDict = new Dictionary<int, Vector3>();
    Dictionary<Vector3, string> nameDict = new Dictionary<Vector3, string>();

    List<int> keyList = new List<int>();

    //불러오기 용
    //아이템의 위치와 종류ID를 담은 valueDict

    //아이템을 배 밖으로 움직였을때 빼버리기용
    //아이템의 포톤뷰ID와 valueDict를 담은 posDict

    private void Start()
    {
        RespawnItems();
    }

    private void OnDisable()
    {
        SaveItems();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            items.Add(collision.gameObject);

            Vector3 itemPos = new Vector3(DistanceCal(collision, 'x'), DistanceCal(collision, 'y'), DistanceCal(collision, 'z')); // 아이템의 위치
            string itemName = collision.gameObject.GetComponent<Item>().name; // 아이템의 종류 ID

            //Clone 문구 지워주기
            string _itemName;
            if (itemName.Contains("(Clone)"))
            {
                _itemName = itemName.Replace("(Clone)", "");
                itemName = _itemName;
            }

            int viewId = collision.gameObject.GetComponent<PhotonView>().ViewID; // 아이템의 포톤뷰 ID
            //불러올때 대비한 키값 저장소
            keyList.Add(viewId);


            nameDict.TryAdd(itemPos, itemName);

            
            posDict.TryAdd(viewId, itemPos);




        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            items.Remove(collision.gameObject);

            int viewId = collision.gameObject.GetComponent<PhotonView>().ViewID;
            
            

            nameDict.Remove(posDict[viewId]);
            posDict.Remove(viewId);
            keyList.Remove(viewId);
        }
    }

    /// <summary>
    /// 거리계산기
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="axis">x y z중 하나 입력</param>
    /// <returns></returns>
    float DistanceCal(Collision collision, char xyz)
    {
        float a = 0f;
        float b = 0f;

        switch (xyz)
        {
            case 'x':
                a = transform.position.x;
                b = collision.transform.position.x;
                break;
            case 'y':
                a = transform.position.y;
                b = collision.transform.position.y;
                break;
            case 'z':
                a = transform.position.z;
                b = collision.transform.position.z;
                break;
        }

        return Mathf.Abs(a - b);
    }

    void SaveItems()
    {
        IngameManager.Instance.posDict = posDict;
        IngameManager.Instance.nameDict = nameDict;
        IngameManager.Instance.keyList = keyList;

    }

    void RespawnItems()
    {
        posDict = IngameManager.Instance.posDict;
        nameDict = IngameManager.Instance.nameDict;
        keyList = IngameManager.Instance.keyList;


        for (int i = 0; i < nameDict.Count; i++)
        {
            PhotonNetwork.Instantiate(nameDict[posDict[keyList[i]]], transform.position + posDict[keyList[i]], Quaternion.identity);
        }
    }
}
