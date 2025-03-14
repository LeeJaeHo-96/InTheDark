using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void Enter(Monster monster);
    void Update();
    void Exit();
}

