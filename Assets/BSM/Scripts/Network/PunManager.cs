using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Lobby Lobby;
    
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

        //Todo: 삭제해야함
        SceneManager.LoadScene("WaitingScene");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("룸 리스트 업데이트"); 
        Lobby.RoomListUpdate(roomList);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("로비 떠남");
        Lobby.ClearRoom();
    }
    
}
