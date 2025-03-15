using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawPos : MonoBehaviourPun
{
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag(Tag.Player);

        photonView.RPC("RPCplayerPosSet", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void RPCplayerPosSet()
    {
        player.transform.position = transform.position;
    }

}
