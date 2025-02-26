using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Host : ObjBind
{

    private Button _publicButton;
    private Button _friendsOnlyButton;
    private Button _makeRoomButton;
    private TextMeshProUGUI _joinNoticeText;
    private TMP_InputField _roomNameText;
    
    private Image _publicButtonImage;
    private Image _friendsOnlyButtonImage;
    
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
 
        _joinNoticeText = GetComponentBind<TextMeshProUGUI>("Join_Notice_Text");
    
        _makeRoomButton.onClick.AddListener(() =>
        {
            HostCreateRoom();
        });
        PublicModeButtonAddListener();
    }

    private void OnEnable()
    {
        _roomNameText.text = $"{GameManager.Instance.CurUser.UserId} 님의 방";
    }


    /// <summary>
    /// 방 공개 여부 버튼 설정
    /// </summary>
    private void PublicModeButtonAddListener()
    {
        _publicButton.onClick.AddListener(() =>
        {
            _publicButtonImage.color = new Color(0f, 0.95f, 1f);
            _friendsOnlyButtonImage.color = Color.white; 
            _publicMode = true;
            _joinNoticeText.text = "공개 방으로 만들면 접속한 \n유저들이 방 리스트를 볼 수 있습니다.";
        });
        
        _friendsOnlyButton.onClick.AddListener(() =>
        {
            _publicButtonImage.color = Color.white;
            _friendsOnlyButtonImage.color = new Color(0f, 0.95f, 1f); 
            
            _publicMode = false;
            _joinNoticeText.text = "비공개 방으로 만들면 초대를 받은 유저들만 접속할 수 있습니다.";
        }); 
        
    }

    private void HostCreateRoom()
    {
        //방이름, 룸옵션,
        PhotonNetwork.CreateRoom(_roomNameText.text, GameManager.Instance.CurRoomOptions);
    }
    
}
