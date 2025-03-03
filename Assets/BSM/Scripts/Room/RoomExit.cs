using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomExit : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(LeaveRoom);
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(); 
    }
}
