using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(RoomOut);
    }

    private void RoomOut()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(3);
    }
}
