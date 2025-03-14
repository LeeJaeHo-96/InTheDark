using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoorOnOff : MonoBehaviourPun
{
    [SerializeField] List<GameObject> lockDoorList;
    [SerializeField] List<GameObject> openDoorList;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (openDoorList.Count > 0)
                CreateDoor(lockDoorList, openDoorList);
            else
                CreateDoor(lockDoorList);
        }
    }

    void CreateDoor(List<GameObject> lockList, List<GameObject> openList)
    {
        bool[] lockStates = new bool[lockList.Count];

        //문 한쪽만 있게 테스트용
        //for (int i = 0; i < lockList.Count; i++)
        for (int i = 0; i < 2; i++)
        {
            if (!lockList[i].activeSelf)
            {
                lockStates[i] = (Random.Range(0, 3) < 0.5f);
                lockList[i].SetActive(lockStates[i]);
            }
            openList[i].SetActive(!lockList[i].activeSelf);
        }

        photonView.RPC("RPCSyncDoors", RpcTarget.Others, lockStates);
    }

    void CreateDoor(List<GameObject> lockList)
    {
        bool[] lockStates = new bool[lockList.Count];

        //문 한쪽만 있게 테스트용
        //for (int i = 0; i < lockList.Count; i++)
        for (int i = 0; i < 2; i++)
        {
            if (!lockList[i].activeSelf)
            {
                lockStates[i] = (Random.Range(0, 3) < 0.5f);
                lockList[i].SetActive(lockStates[i]);
            }
        }

        photonView.RPC("RPCSyncDoors", RpcTarget.Others, lockStates);
    }

    [PunRPC]
    void RPCSyncDoors(bool[] lockStates)
    {
        for (int i = 0; i < lockStates.Length; i++)
        {
            lockDoorList[i].SetActive(lockStates[i]);

            if(openDoorList.Count > i)
            {
                openDoorList[i].SetActive(!lockStates[i]);
            }
        }
    }
}
