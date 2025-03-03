using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    
    public PlayerJump(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        Debug.Log("점프 상태 진입");
    }
    
}
