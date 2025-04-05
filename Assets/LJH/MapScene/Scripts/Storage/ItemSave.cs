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

    //�ҷ����� ��
    //�������� ��ġ�� ����ID�� ���� valueDict

    //�������� �� ������ ���������� ���������
    //�������� �����ID�� valueDict�� ���� posDict

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

            Vector3 itemPos = new Vector3(DistanceCal(collision, 'x'), DistanceCal(collision, 'y'), DistanceCal(collision, 'z')); // �������� ��ġ

            Debug.Log(itemPos);

            string itemName = collision.gameObject.GetComponent<Item>().name; // �������� ���� ID

            //Clone ���� �����ֱ�
            string _itemName;
            if (itemName.Contains("(Clone)"))
            {
                _itemName = itemName.Replace("(Clone)", "");
                itemName = _itemName;

                Debug.Log(itemName);
            }

            int viewId = collision.gameObject.GetComponent<PhotonView>().ViewID; // �������� ����� ID
            //�ҷ��ö� ����� Ű�� �����
            IngameManager.Instance.keyList.Add(viewId);
            IngameManager.Instance.nameDict.TryAdd(itemPos, itemName);
            IngameManager.Instance.posDict.TryAdd(viewId, itemPos);

            Debug.Log($"���ӵ�ųʸ��� ����� ������ ���� : {IngameManager.Instance.nameDict.Count}");

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
           // Debug.Log("������ ���� �����");
        }
    }

    /// <summary>
    /// �Ÿ�����
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="axis">x y z�� �ϳ� �Է�</param>
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
        Debug.Log($"nameDict�� ����{nameDict.Count}");
        IngameManager.Instance.nameDict = nameDict;
        Debug.Log($"�ΰ��� �Ŵ��� nameDict�� ����{IngameManager.Instance.nameDict.Count}");
        IngameManager.Instance.keyList = keyList;

    }

    void RespawnItems()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        posDict = IngameManager.Instance.posDict;
        nameDict = IngameManager.Instance.nameDict;
        keyList = IngameManager.Instance.keyList;

        Debug.Log($"������ �ɶ� �ҷ��� nameDict : { nameDict.Count}");
        Debug.Log($"������ �ɶ� �ҷ��� �ΰ��� �Ŵ��� nameDict : {IngameManager.Instance.nameDict.Count}");

        for (int i = 0; i < nameDict.Count; i++)
        {
            Debug.Log($"������ {i}�� ����");
            PhotonNetwork.Instantiate(nameDict[posDict[keyList[i]]], transform.position + posDict[keyList[i]], Quaternion.identity);
        }
    }
}
