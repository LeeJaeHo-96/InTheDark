using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviourPun
{
    public M_StateMachine stateMachine;

    //플레이어 추적용 게임오브젝트, 콜라이더 관리용 변수
    [SerializeField] List<GameObject> playerList = new List<GameObject>();
    List<Collider> playerColl = new List<Collider>();
    
    public NavMeshAgent agent;

    public Vector3 spawnPointPos;

    float attackDistance = 5f;

    public int Hp = 60;

    public bool isAttacked = false;
    public int pirateDamage = 30;

    private void Start()
    {
        Init();
        StateInit();
    }

    private void Update()
    {
        stateMachine.Update();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tag.Player))
        {
            SearchingPlayer(other);
        }
    }

    void SearchingPlayer(Collider player)
    {
        playerList.Add(player.gameObject);
        playerColl.Add(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (other.CompareTag(Tag.Player))
        {
            Debug.Log("추격 종료");
            playerList.Remove(other.gameObject);
            playerColl.Remove(other);

            //플레이어가 나갔을때 다른 플레이어를 앞 인덱스로 땡겨주는 코드
            for (int i = 0; i < playerList.Count; i++)
            {
                if(playerList[i] == null)
                {
                    playerList.Insert(i, playerList[i + 1]);
                    playerList.Remove(playerList[i+1]);
                }
            }
        }
    }

    /// <summary>
    /// 감지된 플레이어가 있을 경우 True
    /// </summary>
    /// <returns></returns>
    public bool HasPlayers()
    {
        return playerList.Count > 0;
    }

    /// <summary>
    /// 몬스터와 플레이어 거리가 일정 이하로 가까워지면 True 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool PlayerInRange()
    {
        if (playerList.Count == 0) 
            return false;

        //임시로 1f로 둠 추후에 맞춰야함
        return Vector3.Distance(transform.position, playerList[0].transform.position) < attackDistance;
    }

    /// <summary>
    /// 몬스터가 스폰포인트에 오면 True 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsAtSpawnPoint()
    {
        return Vector3.Distance(transform.position, spawnPointPos) < 1f;
    }

    /// <summary>
    /// 플레이어 추격
    /// </summary>
    public void ChasePlayer()
    {
        agent.SetDestination(playerList[0].transform.position);
    }

    /// <summary>
    /// 스폰포인트로 복귀
    /// </summary>
    public void ReturnToSpawn()
    {
        agent.SetDestination(spawnPointPos);
    }

    /// <summary>
    /// 공격 하는 함수
    /// </summary>
    public void AttackStart()
    {
        Debug.Log("공격시작");
        isAttacked = true;
    }

    public void AttackStop()
    {
        Debug.Log("공격끝");
        isAttacked = false;
    }


    void Init()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void StateInit()
    {
        stateMachine = new M_StateMachine();
        stateMachine.ChangeState(new M_IdleState(), this);
    }
}
