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
        StartCoroutine(RespawnItemsCo());
    }

    IEnumerator RespawnItemsCo()
    {
        yield return new WaitForSeconds(0.1f);

        RespawnItems();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            items.Add(collision.gameObject);

            Vector3 itemPos = new Vector3(DistanceCal(collision, 'x'), DistanceCal(collision, 'y'), DistanceCal(collision, 'z')); // 아이템의 위치

            Debug.Log(itemPos);

            string itemName = collision.gameObject.GetComponent<Item>().name; // 아이템의 종류 ID

            //Clone 문구 지워주기
            string _itemName;
            if (itemName.Contains("(Clone)"))
            {
                _itemName = itemName.Replace("(Clone)", "");
                itemName = _itemName;

                Debug.Log(itemName);
            }

            int viewId = collision.gameObject.GetComponent<PhotonView>().ViewID; // 아이템의 포톤뷰 ID
            //불러올때 대비한 키값 저장소
            IngameManager.Instance.keyList.Add(viewId);
            IngameManager.Instance.nameDict.TryAdd(itemPos, itemName);
            IngameManager.Instance.posDict.TryAdd(viewId, itemPos);

            Debug.Log($"네임딕셔너리에 저장된 아이템 갯수 : {IngameManager.Instance.nameDict.Count}");

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
           // items.Remove(collision.gameObject);
           //
           // int viewId = collision.gameObject.GetComponent<PhotonView>().ViewID;
           // 
           // 
           //
           // nameDict.Remove(posDict[viewId]);
           // posDict.Remove(viewId);
           // keyList.Remove(viewId);
           //
           // Debug.Log("아이템 삭제 실행됨");
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

    public void SaveItems()
    {

        IngameManager.Instance.posDict = posDict;
        Debug.Log($"nameDict의 갯수{nameDict.Count}");
        IngameManager.Instance.nameDict = nameDict;
        Debug.Log($"인게임 매니저 nameDict의 갯수{IngameManager.Instance.nameDict.Count}");
        IngameManager.Instance.keyList = keyList;

    }

    void RespawnItems()
    {
        posDict = IngameManager.Instance.posDict;
        nameDict = IngameManager.Instance.nameDict;
        keyList = IngameManager.Instance.keyList;

        Debug.Log($"리스폰 될때 불러온 nameDict : { nameDict.Count}");
        Debug.Log($"리스폰 될때 불러온 인게임 매니저 nameDict : {IngameManager.Instance.nameDict.Count}");

        for (int i = 0; i < nameDict.Count; i++)
        {
            Debug.Log($"아이템 {i}개 생성");
            PhotonNetwork.Instantiate(nameDict[posDict[keyList[i]]], transform.position + posDict[keyList[i]], Quaternion.identity);
        }
    }
}
