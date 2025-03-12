using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : PlayerState
{
    public PlayerDeath(PlayerController controller) : base(controller){}

    public override void Enter()
    {
        _controller.gameObject.SetActive(false);
    }
    
}
