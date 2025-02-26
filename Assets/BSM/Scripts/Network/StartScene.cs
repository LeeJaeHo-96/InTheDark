using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StartScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster 호출");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
        PhotonNetwork.LoadLevel(4);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        
    }
    
}
