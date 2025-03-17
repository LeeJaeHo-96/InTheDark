using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IdleState : IMonsterState
{
    Monster monster;
    public void Enter(Monster monster)
    {
        this.monster = monster;
    }

    public void Update()
    {
        if (monster.HasPlayers())
        {
            monster.stateMachine.ChangeState(new M_ChaseState(), monster);
        }
    }

    public void Exit()
    {
    }
}
