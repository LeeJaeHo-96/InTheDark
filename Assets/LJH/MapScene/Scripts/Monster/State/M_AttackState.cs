using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AttackState : IMonsterState
{
    Monster monster;
    float attackCooldown = 1.5f;
    float lastAttackTime = 0f;

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
        else 
        {
            if (Time.time - lastAttackTime < attackCooldown)
            {
                monster.Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    public void Exit()
    {
    }
}
