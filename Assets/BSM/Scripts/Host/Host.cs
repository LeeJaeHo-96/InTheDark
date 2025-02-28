using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Host : ObjBind
{
    private const string RoomCodeKey = "RoomCode";
    
    private Button _publicButton;
    private Button _friendsOnlyButton;
    private Button _makeRoomButton;
    private TextMeshProUGUI _joinNoticeText;
    private TMP_InputField _roomNameText;
    private TMP_InputField _roomCode;
    
    private Image _publicButtonImage;
    private Image _friendsOnlyButtonImage;

    private RoomOptions _roomOptions;
    
    public bool _publicMode = true;
    
    private void Awake() => Init();
    
    private void Init()
    {
        Bind();
        
        _publicButton = GetComponentBind<Button>("Public_Button");
        _friendsOnlyButton = GetComponentBind<Button>("Friends_Button");
        _makeRoomButton = GetComponentBind<Button>("Confirm_Button");
        
        _publicButtonImage = GetComponentBind<Image>("Public_Button");
        _friendsOnlyButtonImage = GetComponentBind<Image>("Friends_Button");

        _roomNameText = GetComponentBind<TMP_InputField>("RoomName_Field");
        _roomCode = GetComponentBind<TMP_InputField>("Room_Code_Field");
        _joinNoticeText = GetComponentBind<TextMeshProUGUI>("Join_Notice_Text"); 
        OnClickAddListener();
    }

    private void OnEnable()
    {
        _roomNameText.text = $"{GameManager.Instance.CurUser.UserId} 님의 방"; 
    }

    private void OnDisable()
    {
        RoomButtonChange(new Color(0f, 0.95f, 1f), Color.white, true);
        _joinNoticeText.text = "공개 방으로 만들면 접속한 \n유저들이 방 리스트를 볼 수 있습니다.";
    }

    /// <summary>
    /// 버튼 클릭 동작 설정
    /// </summary>
    private void OnClickAddListener()
    {
        _makeRoomButton.onClick.AddListener(() =>
        {
            HostCreateRoom();
        });
        
        _publicButton.onClick.AddListener(() =>
        {
            RoomButtonChange(new Color(0f, 0.95f, 1f), Color.white, true);
            _joinNoticeText.text = "공개 방으로 만들면 접속한 \n유저들이 방 리스트를 볼 수 있습니다.";
        });
        
        _friendsOnlyButton.onClick.AddListener(() =>
        {
            RoomButtonChange(Color.white, new Color(0f, 0.95f, 1f), false);
            _joinNoticeText.text = "비공개 방으로 만들면 초대를 받은 유저들만 접속할 수 있습니다.";
        }); 
        
    }
    
    /// <summary>
    /// 방 설정 변경
    /// </summary>
    /// <param name="publicColor">공개방 버튼 색</param>
    /// <param name="friendsOnlyColor">비공개방 버튼 색</param>
    /// <param name="isPublic">공개방 모드 여부</param>
    private void RoomButtonChange(Color publicColor, Color friendsOnlyColor, bool isPublic)
    {
        _publicButtonImage.color = publicColor;
        _friendsOnlyButtonImage.color = friendsOnlyColor;
        _publicMode = isPublic;
        _roomCode.gameObject.SetActive(_publicMode);
    }
     
    /// <summary>
    /// 방 만들기
    /// </summary>
    private void HostCreateRoom()
    { 
        _roomOptions = new RoomOptions(){MaxPlayers = 4, IsOpen = true, IsVisible = _publicMode};
        //방이름, 룸옵션,
        _roomOptions.CustomRoomProperties = new Hashtable(){{RoomCodeKey, _roomCode.text}};
        _roomOptions.CustomRoomPropertiesForLobby = new string[] {RoomCodeKey};
        Debug.Log(_roomOptions.CustomRoomProperties[RoomCodeKey]);
        PhotonNetwork.CreateRoom(_roomNameText.text, _roomOptions);
    }
    
}
