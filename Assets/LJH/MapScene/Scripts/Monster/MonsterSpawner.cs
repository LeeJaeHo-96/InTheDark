using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviourPun
{
    [SerializeField] List<GameObject> monsterList;
    List<string> monsterNames = new List<string>();

    //게임 시스템의 인게임 타이머와 연동되어야 함 현재 임시로 10 박아두었음
    public int Timer = 10;

    private void Start()
    {
        monsterNames.Add("Enemy1");

        //StartCoroutine(MonsterSpawnCoroutine());
        MonsterSpawn();
    }

    /// <summary>
    /// 특정 시간이 될 때마다 몬스터 스폰
    /// </summary>
    /// <returns></returns>
    IEnumerator MonsterSpawnCoroutine()
    {
        while (true)
        {
            if(Timer % 2 == 1)
            {
                MonsterSpawn();
            }
            yield return null;
        }
    }

    void MonsterSpawn()
    {
        // 몬스터 최소 생성 갯수 계산하기 위한 리스트
        List<GameObject> list = new List<GameObject>();

        Vector3 monPos = gameObject.transform.position + new Vector3(Random.Range(0, 11), 0, Random.Range(0, 11));
        
        if (Random.Range(0, 2) > 0.5f)
        {
            list.Add(PhotonNetwork.Instantiate(monsterNames[Random.Range(0, monsterList.Count)], monPos, Quaternion.identity));
            //몬스터 생성될 때 몬스터 클래스의 생성위치를 지정
            list[list.Count - 1].GetComponent<Monster>().spawnPointPos = monPos;
        }
        //0마리 소환되었을 경우 다시 소환
        if (list.Count == 0)
            MonsterSpawn();
    }
}
