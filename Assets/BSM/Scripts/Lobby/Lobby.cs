using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : ObjBind
{
    private Button _leftLobbyButton;

    private void Awake() => Init();


    private void Init()
    {
        Bind();
        
        _leftLobbyButton = GetComponentBind<Button>("Back_Button");
        OnClickListener();
        
    }

    private void OnClickListener()
    {
        _leftLobbyButton.onClick.AddListener(() => PhotonNetwork.LeaveLobby());
    }
    
}
