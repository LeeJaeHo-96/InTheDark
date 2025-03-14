using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDeath : PlayerState
{
    public PlayerDeath(PlayerController controller) : base(controller){}

    public override void Enter()
    {
        if (_controller.photonView.IsMine)
        {
            Debug.Log("죽었음");
            _controller.photonView.RPC(nameof(_controller.SyncDeathRPC), RpcTarget.AllViaServer, false);
        }
       
    }
 
}
