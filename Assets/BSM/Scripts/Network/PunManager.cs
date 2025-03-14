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
    public List<PlayerController> Players => GameManager.Instance.PlayerObjects;

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
        //방 입장 시 캐릭터 스폰 
        OnChangedPlayer?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnLeftRoom()
    {
        GoToStartScene();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //TODO: 입장 실패 오류 만들어도 ㄱㅊ을듯
        Debug.Log("방 입장 실패");
    }

    /// <summary>
    /// 방에 입장한 상태에서 다른 플레이어가 입장했을 때 호출
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

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
    }

    /// <summary>
    /// 시작 화면으로 이동
    /// </summary>
    private void GoToStartScene()
    {
        PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("StartScene"));
    }
}