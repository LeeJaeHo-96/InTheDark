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
    }

    public override void Update()
    {
        TPS();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("시점 변경");

            ViewChange();

            if (_camIndex > DataManager.Instance.PlayerObjects.Count - 1)
            {
                _camIndex = 0;
            }
        }
    }

    private void TPS()
    {
        ViewChange();

        _mouseX += Input.GetAxisRaw("Mouse X");
        _mouseY -= Input.GetAxisRaw("Mouse Y");
        
        Vector3 charPos = new Vector3(
            DataManager.Instance.PlayerObjects[_camIndex].PosX,
            DataManager.Instance.PlayerObjects[_camIndex].PosY, 
            DataManager.Instance.PlayerObjects[_camIndex].PosZ
            );

         _controller.PlayerCam.transform.position = charPos + new Vector3(0, 1f, 10);
         _controller.PlayerCam.transform.rotation = Quaternion.LookRotation(charPos);
    }

    private void ViewChange()
    {
        if (DataManager.Instance.PlayerObjects[_camIndex].IsDeath)
        {
            while (DataManager.Instance.PlayerObjects[_camIndex].IsDeath)
            {
                _camIndex++;
            }
        }
    }
}