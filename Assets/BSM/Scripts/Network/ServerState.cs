using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ServerState : MonoBehaviourPunCallbacks
{

    private ClientState _state;

    private void Update()
    {
        if (_state == PhotonNetwork.NetworkClientState) return;
        
        _state = PhotonNetwork.NetworkClientState;

        Debug.Log($"state :{_state}");
    }
}
