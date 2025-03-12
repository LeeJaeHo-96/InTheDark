using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] GameObject pirate;
    Monster monster;

    private void Start()
    {
        monster = pirate.GetComponent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(monster.state == monsterState.attack)
            if(other.CompareTag(Tag.Player))
            {
                //Todo : 플레이어의 체력을 감소해야함
                //other.GetComponent<PlayerStat>().hp -= 1;
                Debug.Log("몬스터가 플레이어 공격");
            }
    }
}
