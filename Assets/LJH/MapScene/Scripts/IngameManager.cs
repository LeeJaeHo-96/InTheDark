using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class IngameManager : MonoBehaviourPun
{
    public static IngameManager Instance;

    public float time { get; set; }
    public float minute { get; set; }

    public int days { get; set; }

    float randomMinute;

    float elapsedTime = 1f;

    public int money { get; set; }

    public string masterID;

    //아이템 위치 저장용 ItemSave 스크립트와 연동
    public Dictionary<int, Vector3> posDict = new Dictionary<int, Vector3>();
    public Dictionary<Vector3, string> nameDict = new Dictionary<Vector3, string>();
    [HideInInspector]  public List<int> keyList = new List<int>();


    //게임오버 용
    [SerializeField] GameObject gameOverPopup;
    int playerCount;


    private void Awake()
    {
        Init();
        SingletonInit();
    }

    private void Start()
    {
        Invoke(nameof(PlayerCheck), 5f);
    }
    void PlayerCheck()
    {
        playerCount = GameManager.Instance.PlayerObjects.Count;
        Debug.Log($"현재 인원 {playerCount}");
    }

    private void Update()
    {
        if (playerCount != 0)
        {
            if (playerCount - GameManager.Instance.PlayerObjects.Count == playerCount)
            {
                GameOver();
            }
        }
    }


    //시간의 경우 10초마다 5 ~10분이 흐르고
    //am8시로 시작해서
    //pm8시되면 배 침몰

    /// <summary>
    /// 타이머 리셋 - 맵씬 시작될 때 호출
    /// </summary>
    public void TimerReset()
    {
        time = 8;
        minute = 0;
    }

    /// <summary>
    /// 시간 계산기 - 맵씬 시작될 때 호출
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimerCount()
    {
        while (true) 
        {
            randomMinute = Random.Range(5, 10);
            minute += randomMinute;

            if (minute >= 60)
            {
                time++;
                minute = minute - 60;
            }

            if(time >= 12)
            {
                TimeOver();
            }
            yield return new WaitForSeconds(elapsedTime);
        }
    }

    void TimeOver()
    {
        photonView.RPC("RPCPlayerAllDie", RpcTarget.AllViaServer);

    }

    [PunRPC]
    void RPCPlayerAllDie()
    {
        List<GameObject> playerS = new List<GameObject>();

        playerS = GameObject.FindGameObjectsWithTag(Tag.Player).ToList();

        foreach (GameObject player in playerS)
        {
            player.GetComponent<PlayerController>().ChangeState(PState.DEATH);
        }
    }

    [PunRPC]
    void GameOver()
    {
        gameOverPopup.SetActive(true);
    }

    void Init()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"방장이라 내꺼로 접근");
            Database.instance.LoadData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_1");

        }
        else
        {
            Debug.Log($"{masterID}로 접근");
            Database.instance.LoadData(masterID, "Slot_1");
        }
    }


    void SingletonInit()
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
}
