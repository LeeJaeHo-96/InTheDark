using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AttackState : IMonsterState
{
    Monster monster;

    public void Enter(Monster monster)
    {
        this.monster = monster;
    }

    public void Update()
    {
        if(!monster.PlayerInRange())
        {
            monster.stateMachine.ChangeState(new M_ChaseState(), monster);
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
