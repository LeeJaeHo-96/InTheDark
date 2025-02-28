using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : ObjBind
{

    private RectTransform _roomContentRect;
    private RoomJoin _roomJoinPrefab;
    private Dictionary<string, RoomJoin> _roomJoinDict = new Dictionary<string, RoomJoin>();
    
    private Button _leftLobbyButton;
        
    private void Awake() => Init();
 
    private void Init()
    {
        Bind();

        _roomContentRect = GetComponentBind<RectTransform>("Room_Content");
        _leftLobbyButton = GetComponentBind<Button>("Back_Button");
        OnClickListener();
        _roomJoinPrefab = Resources.Load<RoomJoin>("Room_Prefabs");
    } 
    
    private void OnClickListener()
    {
        _leftLobbyButton.onClick.AddListener(() => PhotonNetwork.LeaveLobby()); 
    }

    public void RoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList || !info.IsVisible || !info.IsOpen)
            {
                if(!_roomJoinDict.ContainsKey(info.Name)) continue;
                
                Destroy(_roomJoinDict[info.Name].gameObject);
                _roomJoinDict.Remove(info.Name);
            } 
            else if (!_roomJoinDict.ContainsKey(info.Name))
            {
                RoomJoin roomJoin = Instantiate(_roomJoinPrefab, _roomContentRect);
                _roomJoinDict[info.Name] = roomJoin;
                roomJoin.SetRoomInfo(info);
            }
            else if (_roomJoinDict.ContainsKey(info.Name))
            {
                RoomJoin roomJoin = _roomJoinDict[info.Name];
                roomJoin.SetRoomInfo(info);
            }
        }
        
        
    }

    public void ClearRoom()
    {
        
    }
    
}
 