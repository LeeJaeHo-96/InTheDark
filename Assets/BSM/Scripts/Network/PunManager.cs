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
    private Lobby Lobby => Lobby.Instance;
    public static PunManager Instance;
    public UnityAction OnChangedPlayer;
    public List<PlayerController> Players => DataManager.Instance.PlayerObjects;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    }

    public override void OnCreatedRoom()
    { 
        GoToWaitingScene();
    }

    public override void OnJoinedRoom()
    {
        OnChangedPlayer?.Invoke();
        //TODO: 테스트가 끝나면 풀것
        //Cursor.lockState = CursorLockMode.Locked;
        
        //TODO: 방장이 하나 추가하고
    }

    public override void OnLeftRoom()
    {
        GoToStartScene();
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방 입장 실패");
    }
    
    /// <summary>
    /// 방에 입장한 상태에서 다른 플레이어가 입장했을 때 호출
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //TODO: 방장일 때 한명씩 추가?
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //TODO: 방장일 때 한명씩 삭제
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnJoinedLobby()
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Lobby.RoomListUpdate(roomList);
    }

    public override void OnLeftLobby()
    {
        Lobby.ClearRoom();
    }

    /// <summary>
    /// 대기 화면으로 이동
    /// </summary>
    private void GoToWaitingScene()
    {
        PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("WaitingScene"));
        
        //TODO: 게임 넘어갈 때, 다른 유저들도 lIST 업데이트 하면 될 것 같음
    }
    
    /// <summary>
    /// 시작 화면으로 이동
    /// </summary>
    private void GoToStartScene()
    {
        PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("StartScene"));

    }
    
}
