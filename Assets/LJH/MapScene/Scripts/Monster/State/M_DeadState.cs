using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DeadState : IMonsterState
{
    Monster monster;
    public void Enter(Monster monster)
    {
        this.monster = monster;

    }

    public void Update()
    {
        
    }

    public void Exit()
    {
    }
}
