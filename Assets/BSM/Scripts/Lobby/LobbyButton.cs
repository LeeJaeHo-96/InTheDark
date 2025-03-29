using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour
{
    private Button _lobbyButton;

    private void Awake()
    {
        _lobbyButton = GetComponent<Button>();
        
        _lobbyButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySfx(SoundManager.Instance.SoundDatas.SoundDict["ButtonClickSFX"]);
        });
    }
}
