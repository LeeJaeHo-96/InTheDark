using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDeath : PlayerState
{
    private float _mouseX;
    private float _mouseY;

    private int _camIndex = -1;

    public PlayerDeath(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    { 
        _controller.BehaviourAnimation(_deathAniHash);
        
        if (_controller.photonView.IsMine)
        {
            _controller.IsDeath = true;
            _controller.PlayerCanvas.gameObject.SetActive(false);
            _controller.photonView.RPC(nameof(_controller.SyncDeathRPC), RpcTarget.AllViaServer, true, true, false);
            _controller.PlayerCam.transform.SetParent(null);
        }

        AliveCharacterSearch();
    }

    public override void Update()
    {
        FollowCharacter();

        if (Input.GetMouseButtonDown(0))
        {
            AliveCharacterSearch();
        }
    }
    
    /// <summary>
    /// 생존 캐릭터 검색
    /// </summary>
    private void AliveCharacterSearch()
    {
        do
        {
            _camIndex = (_camIndex + 1) % DataManager.Instance.PlayerObjects.Count; 
            
        } while (DataManager.Instance.PlayerObjects[_camIndex].IsDeath);
    }
    
    /// <summary>
    /// 카메라가 추적할 캐릭터 위치
    /// </summary>
    private void FollowCharacter()
    {
        _controller.ObserverPos = DataManager.Instance.PlayerObjects[_camIndex].PlayerBody.transform.position;
    }

}