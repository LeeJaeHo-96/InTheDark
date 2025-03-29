using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PunManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PunVoiceClient punVoiceClient;
    private Lobby Lobby => Lobby.Instance;
    public static PunManager Instance;
    public UnityAction OnChangedPlayer;
    
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
        PhotonNetwork.ConnectUsingSettings();
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

        if (GameManager.Instance.PlayerSearchCo == null)
        {
            GameManager.Instance.PlayerSearchCo = StartCoroutine(GameManager.Instance.PlayerSearchRoutine());
        }
        GameManager.Instance.SceneBGM(SceneType.WAITING);

        //이재호 추가 코드 : Invoke를 통해 게임속에서만 존재하는 IngameManager에 주입시킴
        Invoke("InsertMasterID", 1f);
        
    }

    void InsertMasterID()
    {
        //이재호 추가 코드 : MasterID 변수에 마스터클라이언트의 유저아이디를 주입
        if (PhotonNetwork.IsMasterClient)
        {
            IngameManager.Instance.masterID = FirebaseManager.Auth.CurrentUser.UserId;
        }
    }

    public override void OnLeftRoom()
    {
        if (GameManager.Instance.PlayerSearchCo != null)
        {
             StopCoroutine(GameManager.Instance.PlayerSearchCo);
             GameManager.Instance.PlayerSearchCo = null;
        }
        
        
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
        //GameManager.Instance.PlayerObjects.Clear();
        //GameManager.Instance.PlayerObjects = FindObjectsOfType<PlayerController>().ToList();
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
    public void GoToStartScene()
    {
        punVoiceClient.Disconnect();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GameManager.Instance.SceneBGM(SceneType.MAIN);
        PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("StartScene"));
    }
}