using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ChaseState : IMonsterState
{
    Monster monster;
    public void Enter(Monster monster)
    {
        this.monster = monster;
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

        if (monster.Hp <= 0)
        {
            monster.stateMachine.ChangeState(new M_DeadState(), monster);
        }

    }

    public void Exit()
    {
    }
}
