using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ItemSpawner : MonoBehaviourPun
{
    List<string> itemList = new List<string>();
    Vector3 rayPos;
    float xDistance;
    float zDistance;
    private void Start()
    {
        //아이템 담아줘야함
        itemList.Add("Barrel");
        itemList.Add("VintageTelephone");
        itemList.Add("Gramophone");

        rayPos = transform.position + new Vector3(0, 1, 0);

        xDistance = DistanceCal(Vector3.forward);
        zDistance = DistanceCal(Vector3.right);

        SpawnItem();
    }

    private void Update()
    {
        //레이캐스트 테스트용
        //Debug.DrawRay(rayPos, Vector3.forward * 30, Color.red);
        //Debug.DrawRay(rayPos, Vector3.right * 30, Color.red);
    }

    /// <summary>
    /// 거리 계산기
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    float DistanceCal(Vector3 dir)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(rayPos, dir, out hitInfo, 30.0f))
        {
            if (dir == Vector3.forward)
                return transform.position.x - hitInfo.transform.position.x;

            else if (dir == Vector3.right)
                return transform.position.z - hitInfo.transform.position.z;
        }

        return 0;
    }

    /// <summary>
    /// 아이템 생성
    /// </summary>
    void SpawnItem()
    {
        int items = Random.Range(0, 3);
       

        for (int i = 0; i < items; i++)
        {
            // 랜덤으로 아이템과 생성위치 지정해줌
            string item = itemList[Random.Range(0, itemList.Count)];
            Vector3 pos = transform.position + new Vector3(Random.Range(-xDistance, xDistance), 0, Random.Range(-zDistance, zDistance));

            //Instantiate(item, pos, Quaternion.identity);
            PhotonNetwork.Instantiate(item, pos, Quaternion.identity);
        }
    }
}
