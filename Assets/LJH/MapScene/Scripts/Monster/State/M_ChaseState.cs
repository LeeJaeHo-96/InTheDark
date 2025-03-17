using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ChaseState : IMonsterState
{
    Monster monster;
    public void Enter(Monster monster)
    {
        this.monster = monster;
        Debug.Log("추격 상태 진입");
    }

    public void Update()
    {
        Debug.Log(monster.PlayerInRange());

        if (monster.PlayerInRange())
        {
            monster.stateMachine.ChangeState(new M_AttackState(), monster);
        }

        if (monster.HasPlayers())
        {
            monster.ChasePlayer();
        }
        if (!monster.HasPlayers())
        {
            monster.stateMachine.ChangeState(new M_ReturnState(), monster);
        }

    }

    public void Exit()
    {
        Debug.Log("추격 상태 나감");
    }
}
