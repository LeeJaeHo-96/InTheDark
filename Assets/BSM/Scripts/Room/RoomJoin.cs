using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomJoin : ObjBind
{
    private TextMeshProUGUI _roomName;
    private TextMeshProUGUI _roomPeopleCount;
    private Button _roomJoinButton;

    private void Awake() => Init();

    private void Init()
    {
        Bind();
        
        _roomName = GetComponentBind<TextMeshProUGUI>("Lobby_Room_Name");
        _roomPeopleCount = GetComponentBind<TextMeshProUGUI>("Lobby_Room_People");
        _roomJoinButton = GetComponentBind<Button>("Looby_Room_Join");

        OnClickAddListener();
    }

    private void OnClickAddListener()
    {
        _roomJoinButton.onClick.AddListener(Join);
    }

    /// <summary>
    /// 로비에서 방 정보
    /// </summary>
    /// <param name="info"></param>
    public void SetRoomInfo(RoomInfo info)
    {
        _roomName.text = info.Name;
        _roomPeopleCount.text = $"{info.PlayerCount} / {info.MaxPlayers}";

        _roomJoinButton.interactable = info.PlayerCount < info.MaxPlayers;
    }
    
    /// <summary>
    /// 해당하는 방제목의 방 입장
    /// </summary>
    private void Join()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Instance.SoundDatas.SoundDict["ButtonClickSFX"]);
        PhotonNetwork.JoinRoom(_roomName.text);
    }
    
}
