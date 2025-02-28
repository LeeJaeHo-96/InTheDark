using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : ObjBind
{
    private Dictionary<string, RoomJoin> _roomJoinDict = new Dictionary<string, RoomJoin>();
    private Dictionary<string, string> _roomCodeDict = new Dictionary<string, string>();
    
    private List<RoomInfo> _roomList = new List<RoomInfo>();
    private RoomJoin _roomJoinPrefab;

    private TMP_InputField _roomCode;
    private RectTransform _roomContentRect; 
    private Button _refreshButton;
    private Button _leftLobbyButton;
    private string _roomCodeStr = "";    
    
    private void Awake() => Init();
 
    private void Init()
    {
        Bind();

        _roomContentRect = GetComponentBind<RectTransform>("Room_Content");
        _refreshButton = GetComponentBind<Button>("Refresh_Button");
        _leftLobbyButton = GetComponentBind<Button>("Back_Button");
        _roomCode = GetComponentBind<TMP_InputField>("Room_Code_Field");
        OnClickListener();
        
        _roomJoinPrefab = Resources.Load<RoomJoin>("Room_Prefabs");
    } 
    
    private void OnClickListener()
    {
        _leftLobbyButton.onClick.AddListener(() => PhotonNetwork.LeaveLobby()); 
        _refreshButton.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinLobby(); 
        });
        
        _roomCode.onValueChanged.AddListener(x => RoomCodeCompare(x)); 
    }

    private void RoomCodeCompare(string x)
    {
        foreach (RoomInfo room in _roomList)
        {

            if (!_roomCodeDict.ContainsKey(room.CustomProperties["RoomCode"].ToString()))
            {
                _roomCodeStr = room.CustomProperties["RoomCode"].ToString();
            }
            
            if (x.Length > 0)
            {
                if (!_roomCodeStr.Equals(x))
                {
                    _roomJoinDict[room.Name].gameObject.SetActive(false);
                }
                else
                {
                    _roomJoinDict[room.Name].gameObject.SetActive(true);
                }
            }
            else
            {
                _roomJoinDict[room.Name].gameObject.SetActive(true);
            }
        }
    }
    
    /// <summary>
    /// 로비 입장 시 방 정보 갱신
    /// </summary>
    /// <param name="roomList"></param>
    public void RoomListUpdate(List<RoomInfo> roomList)
    {
        _roomList = roomList;
        
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

    /// <summary>
    /// 기존 방 리스트 삭제
    /// </summary>
    public void ClearRoom()
    {
        foreach (string name in _roomJoinDict.Keys)
        {
            Destroy(_roomJoinDict[name].gameObject);
        }
        
        _roomJoinDict.Clear();
    }
    
}
 