using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum monsterState
{
    idle,
    chase,
    attack,
    returnMove
}
public class Monster : MonoBehaviourPun
{
    public monsterState state;

    [SerializeField] List<GameObject> playerList = new List<GameObject>();
    List<Collider> playerColl = new List<Collider>();
    NavMeshAgent agent;

    [SerializeField] GameObject spawnPoint;
    Vector3 spawnPointPos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPointPos = GameObject.FindWithTag(Tag.MonsterSpawner).transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tag.Player))
        {
            Debug.Log("인식 완료");
            playerList.Add(other.gameObject);
            playerColl.Add(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //플레이어 추격
        if(other.CompareTag(Tag.Player))
        {
            Debug.Log("추격중");
            agent.SetDestination(other.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Tag.Player))
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
            //원래 위치로 돌아감
            agent.SetDestination(spawnPointPos);
        }
    }
}
