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
        if (_controller.photonView.IsMine)
        {
            _controller.IsDeath = true;
            _controller.PlayerCanvas.gameObject.SetActive(false);
            _controller.photonView.RPC(nameof(_controller.SyncDeathRPC), RpcTarget.AllViaServer, true, false, true,
                false);
            _controller.PlayerCam.transform.SetParent(null);
        }

        AliveCharacterSearch();
    }

    public override void Update()
    {
        TPS();

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
            _camIndex = (_camIndex + 1) % GameManager.Instance.PlayerObjects.Count;
            
        } while (GameManager.Instance.PlayerObjects[_camIndex].IsDeath);
    }
    
    /// <summary>
    /// 3인칭 시점
    /// </summary>
    private void TPS()
    {
        _mouseX += Input.GetAxisRaw("Mouse X");
        _mouseY -= Input.GetAxisRaw("Mouse Y");
        
        Vector3 charPos = new Vector3(
            GameManager.Instance.PlayerObjects[_camIndex].PosX,
            GameManager.Instance.PlayerObjects[_camIndex].PosY, 
            GameManager.Instance.PlayerObjects[_camIndex].PosZ
            );
        
        //카메라 상,하 제한
        _mouseY = Mathf.Clamp(_mouseY, -50f, 20f);
        Quaternion rot = Quaternion.Euler(_mouseY, _mouseX, 0);
        
        //캐릭터와 카메라의 여유 거리
        Vector3 direction = new Vector3(0, 3f, -3f);
        _controller.PlayerCam.transform.position = charPos + rot * direction;
        
        //카메라 고정
        _controller.PlayerCam.transform.LookAt(charPos);
    }

}