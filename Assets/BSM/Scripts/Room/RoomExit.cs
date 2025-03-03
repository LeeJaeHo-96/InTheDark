using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomExit : MonoBehaviour
{
    [Header("방 나가기 버튼")]
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
