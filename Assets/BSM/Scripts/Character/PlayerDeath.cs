using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDeath : PlayerState
{
    private float _mouseX;
    private float _mouseY;

    private int _camIndex = 0;

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

        for (int i = 0; i < DataManager.Instance.PlayerObjects.Count; i++)
        {
            if (!DataManager.Instance.PlayerObjects[i].IsDeath)
            {
                _camIndex = i;
                break;
            } 
        } 
    }

    public override void Update()
    {
        TPS();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("시점 변경");

            _camIndex++;
            
            if (_camIndex > DataManager.Instance.PlayerObjects.Count - 1)
            {
                _camIndex = 0;
            }
            
            if (DataManager.Instance.PlayerObjects[_camIndex].IsDeath)
            {
                for (int i = _camIndex; i < DataManager.Instance.PlayerObjects.Count; i++)
                {
                    if (!DataManager.Instance.PlayerObjects[i].IsDeath)
                    {
                        _camIndex = i;
                        break;
                    }
                }  
            } 
        }
    }

    private void TPS()
    {
        _mouseX += Input.GetAxisRaw("Mouse X");
        _mouseY -= Input.GetAxisRaw("Mouse Y");
        
        Vector3 charPos = new Vector3(
            DataManager.Instance.PlayerObjects[_camIndex].PosX,
            DataManager.Instance.PlayerObjects[_camIndex].PosY, 
            DataManager.Instance.PlayerObjects[_camIndex].PosZ
            );
        
        _mouseY = Mathf.Clamp(_mouseY, -50f, 20f);
        Quaternion rot = Quaternion.Euler(_mouseY, _mouseX, 0);
        Vector3 direction = new Vector3(0, 3f, -3f);
        _controller.PlayerCam.transform.position = charPos + rot * direction;
        
        _controller.PlayerCam.transform.LookAt(charPos);
    }

}