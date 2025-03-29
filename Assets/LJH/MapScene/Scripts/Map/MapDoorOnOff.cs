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
        //방
        if (openDoorList.Count > 0)
        {
            CreateDoor(lockDoorList, openDoorList);
        }
        //복도
        else
        {
            if (PhotonNetwork.IsMasterClient)
                CreateDoor(lockDoorList);
        }
    }

    /// <summary>
    /// 방 의 경우 이걸로
    /// </summary>
    /// <param name="lockList"></param>
    /// <param name="openList"></param>
    void CreateDoor(List<GameObject> lockList, List<GameObject> openList)
    {

        for (int i = 0; i < lockList.Count - 2; i++)
        {
            openList[i].SetActive(!lockList[i].activeSelf);
        }

    }

    /// <summary>
    /// 복도의 경우 이걸로
    /// </summary>
    /// <param name="lockList"></param>
    void CreateDoor(List<GameObject> lockList)
    {
        int howManyLocked;

        howManyLocked = Random.Range(0, lockList.Count);
        for (int i = 0; i < howManyLocked; i++)
        {
            lockList[i].SetActive(true);
        }

        photonView.RPC(nameof(RPCSyncDoors), RpcTarget.Others, howManyLocked);

    }

    [PunRPC]
    void RPCSyncDoors(int howManyLocked)
    {
        for (int i = 0; i < howManyLocked; i++)
        {
            lockDoorList[i].SetActive(true);
        }
    }

}
