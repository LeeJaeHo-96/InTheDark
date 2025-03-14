using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_StateMachine
{
    private IMonsterState currentState;

    public void ChangeState(IMonsterState newState, Monster monster)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(monster);
    }

    public void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
    }

}
