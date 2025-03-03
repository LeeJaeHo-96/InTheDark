using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PunManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Lobby Lobby => Lobby.Instance;

    public static PunManager Instance;
    
    public UnityAction OnChangedPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("WaitingScene"));
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        OnChangedPlayer?.Invoke();
    }

    public override void OnLeftRoom()
    {
        OnChangedPlayer = null;
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방 입장 실패");
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("엔터드 룸");
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
