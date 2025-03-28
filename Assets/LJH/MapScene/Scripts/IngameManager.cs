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
        StartCoroutine(curPlayerCheckCoroutine());
    }

    private void Update()
    {
        //Todo : 업데이트라 좀 부담시러움.. 딴걸로 바꾸면 좋을거 같음
        if (gameOverPopup.activeSelf) return;

    }

    IEnumerator curPlayerCheckCoroutine()
    {
        yield return new WaitForSecondsRealtime(3f);
        Debug.Log("인원 체크 시작");
        while (true)
        {
            Debug.Log("인원 체크 도는중");
            List<GameObject> playerList = new List<GameObject>();

            playerList = GameObject.FindGameObjectsWithTag(Tag.Player).ToList();
            int playerCount = 0;
            foreach(GameObject pl in playerList)
            {
                if (pl.GetComponent<PlayerController>().IsDeath == true)
                    playerCount++;
            }

            if(GameManager.Instance.PlayerObjects.Count == playerCount)
            {
                GameOver();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PlayerCheck()
    {
        playerCount = GameManager.Instance.PlayerObjects.Count;
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

            if(time >= 24)
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

    public void GameOver()
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
