using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_StateMachine
{
    private IMonsterState currentState;
    Animator animator;


    public void ChangeState(IMonsterState newState, Monster monster)
    {
        animator = monster.GetComponent<Animator>();

        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(monster);
        AnimationPlay(newState, monster);

        Debug.Log($"{newState} ∑Œ ¡¯¿‘µ ");
    }

    void AnimationPlay(IMonsterState newState, Monster monster)
    {
        switch(newState)
        {
            case M_IdleState :
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", false);
                break;

            case M_ChaseState:
            case M_ReturnState:
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalk", true);
                animator.SetBool("isAttack", false);
                break;

            case M_AttackState:
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", true);
                break;
        }
    }

    public void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
    }

}
